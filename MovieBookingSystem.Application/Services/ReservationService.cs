using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Common.Exceptions;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto, Guid userId)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto));
            }

            // Usar o userId do parâmetro em vez do DTO
            var actualUserId = userId != Guid.Empty ? userId : createDto.UserId;

            // Verificar se o usuário existe
            var user = await _unitOfWork.Users.GetByIdAsync(actualUserId);
            if (user == null)
            {
                throw new NotFoundException("User not found", actualUserId);
            }

            // Verificar se o showtime existe e incluir relacionamentos necessários
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(createDto.ShowtimeId);
            if (showtime == null)
            {
                throw new NotFoundException("Showtime not found", createDto.ShowtimeId);
            }

            // Verificar se os assentos existem
            if (createDto.SeatIds == null || !createDto.SeatIds.Any())
            {
                throw new ArgumentException("At least one seat must be selected");
            }

            var seats = await _unitOfWork.Seats.GetByIdsAsync(createDto.SeatIds);
            if (seats.Count != createDto.SeatIds.Count)
            {
                throw new NotFoundException("One or more seats not found", createDto.SeatIds);
            }

            // Verificar a disponibilidade dos assentos
            foreach (var seat in seats)
            {
                if (seat.IsReserved || !seat.IsAvailable)
                {
                    // Especificar qual ApplicationException usar para evitar ambiguidade
                    throw new MovieBookingSystem.Application.Common.Exceptions.ApplicationException($"Seat {seat.Row}{seat.SeatNumber} is not available");
                }
            }

            // Calcular preço total
            decimal totalPrice = showtime.Price * seats.Count;

            // Criar entidade de reserva
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = actualUserId,
                ShowtimeId = createDto.ShowtimeId,
                ReservationDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationSeats = createDto.SeatIds.Select(seatId => new ReservationSeat
                {
                    Id = Guid.NewGuid(),
                    SeatId = seatId,
                    ReservationId = Guid.Empty // Será definido após salvar a reserva
                }).ToList()
            };

            // Definir o ReservationId após criar a reserva
            foreach (var reservationSeat in reservation.ReservationSeats)
            {
                reservationSeat.ReservationId = reservation.Id;
            }

            // Adicionar reserva ao repositório
            await _unitOfWork.Reservations.AddAsync(reservation);

            // Marcar assentos como reservados
            foreach (var seat in seats)
            {
                seat.IsReserved = true;
                await _unitOfWork.Seats.UpdateAsync(seat);
            }

            // Salvar mudanças no banco de dados
            await _unitOfWork.CompleteAsync();

            // Mapear a reserva criada para DTO
            var reservationDto = _mapper.Map<ReservationDto>(reservation);

            // Enriquecer DTO com informações do usuário, título do filme e detalhes dos assentos
            reservationDto.UserName = user.UserName;
            reservationDto.MovieTitle = showtime.Movie?.Title ?? "Unknown Movie";
            reservationDto.ShowtimeStart = showtime.StartTime;
            reservationDto.Seats = seats.Select(s => new SeatDto
            {
                Id = s.Id,
                ShowtimeId = s.ShowtimeId,
                Row = s.Row,
                SeatNumber = s.SeatNumber,
                IsReserved = s.IsReserved
            }).ToList();

            return reservationDto;
        }

        public async Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found", userId);

            var reservations = await _unitOfWork.Reservations.GetByUserIdAsync(userId);

            var reservationDtos = reservations.Select(r =>
            {
                var dto = _mapper.Map<ReservationDto>(r);
                dto.UserName = user.UserName;
                dto.MovieTitle = r.Showtime?.Movie?.Title ?? "Unknown Movie";
                dto.ShowtimeStart = r.Showtime?.StartTime ?? default;
                dto.Seats = r.ReservationSeats?.Select(rs => new SeatDto
                {
                    Id = rs.SeatId,
                    ShowtimeId = rs.Seat?.ShowtimeId ?? Guid.Empty,
                    Row = rs.Seat?.Row ?? "?",
                    SeatNumber = rs.Seat?.SeatNumber ?? 0,
                    IsReserved = rs.Seat?.IsReserved ?? false
                }).ToList() ?? new List<SeatDto>();
                return dto;
            });

            return reservationDtos;
        }

        public async Task<ReservationDto> CancelReservationAsync(Guid reservationId)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found", reservationId);

            if (reservation.Status == ReservationStatus.Cancelled)
                // Especificar qual ApplicationException usar para evitar ambiguidade
                throw new MovieBookingSystem.Application.Common.Exceptions.ApplicationException("Reservation is already cancelled");

            reservation.Status = ReservationStatus.Cancelled;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Reservations.UpdateAsync(reservation);

            // Liberar assentos reservados
            foreach (var rs in reservation.ReservationSeats)
            {
                var seat = await _unitOfWork.Seats.GetByIdAsync(rs.SeatId);
                if (seat != null)
                {
                    seat.IsReserved = false;
                    await _unitOfWork.Seats.UpdateAsync(seat);
                }
            }

            await _unitOfWork.CompleteAsync();

            var user = await _unitOfWork.Users.GetByIdAsync(reservation.UserId);
            var dto = _mapper.Map<ReservationDto>(reservation);
            dto.UserName = user?.UserName ?? "Unknown User";
            dto.MovieTitle = reservation.Showtime?.Movie?.Title ?? "Unknown Movie";
            dto.ShowtimeStart = reservation.Showtime?.StartTime ?? default;
            dto.Seats = reservation.ReservationSeats?.Select(rs => new SeatDto
            {
                Id = rs.SeatId,
                ShowtimeId = rs.Seat?.ShowtimeId ?? Guid.Empty,
                Row = rs.Seat?.Row ?? "?",
                SeatNumber = rs.Seat?.SeatNumber ?? 0,
                IsReserved = false
            }).ToList() ?? new List<SeatDto>();

            return dto;
        }

        public async Task DeleteAsync(Guid reservationId)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found", reservationId);

            if (reservation.Status != ReservationStatus.Cancelled)
            {
                foreach (var rs in reservation.ReservationSeats)
                {
                    var seat = await _unitOfWork.Seats.GetByIdAsync(rs.SeatId);
                    if (seat != null)
                    {
                        seat.IsReserved = false;
                        await _unitOfWork.Seats.UpdateAsync(seat);
                    }
                }
            }

            await _unitOfWork.Reservations.DeleteAsync(reservationId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ReservationDto> GetByIdAsync(Guid reservationId)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found", reservationId);

            var user = await _unitOfWork.Users.GetByIdAsync(reservation.UserId);

            var dto = _mapper.Map<ReservationDto>(reservation);
            dto.UserName = user?.UserName ?? "Unknown User";
            dto.MovieTitle = reservation.Showtime?.Movie?.Title ?? "Unknown Movie";
            dto.ShowtimeStart = reservation.Showtime?.StartTime ?? default;
            dto.Seats = reservation.ReservationSeats?.Select(rs => new SeatDto
            {
                Id = rs.SeatId,
                ShowtimeId = rs.Seat?.ShowtimeId ?? Guid.Empty,
                Row = rs.Seat?.Row ?? "?",
                SeatNumber = rs.Seat?.SeatNumber ?? 0,
                IsReserved = rs.Seat?.IsReserved ?? false
            }).ToList() ?? new List<SeatDto>();

            return dto;
        }
    }
}