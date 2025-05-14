using AutoMapper;
using MovieBookingSystem.Application.DTOs;
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

        public async Task<ReservationDto> GetByIdAsync(Guid id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetByUserIdAsync(Guid userId)
        {
            var reservations = await _unitOfWork.Reservations.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

        public async Task<IEnumerable<ReservationDto>> GetByShowtimeIdAsync(Guid showtimeId)
        {
            var reservations = await _unitOfWork.Reservations.GetByShowtimeIdAsync(showtimeId);
            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationDto createDto)
        {
            // Verificar se o usuário existe
            var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
            if (user == null)
                throw new ApplicationException("User not found");

            // Verificar se o showtime existe
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(createDto.ShowtimeId);
            if (showtime == null)
                throw new ApplicationException("Showtime not found");

            // Verificar se os assentos existem e pertencem ao showtime correto
            var seats = await _unitOfWork.Seats.GetByIdsAsync(createDto.SeatIds);

            if (seats.Count() != createDto.SeatIds.Count())
                throw new ApplicationException("One or more seats not found");

            if (seats.Any(s => s.ShowtimeId != createDto.ShowtimeId))
                throw new ApplicationException("One or more seats don't belong to the selected showtime");

            // Verificar se os assentos estão disponíveis
            if (seats.Any(s => s.IsReserved))
                throw new ApplicationException("One or more seats are already reserved");

            // Calcular o preço total
            decimal totalPrice = showtime.Price * seats.Count();

            // Criar a reserva
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = createDto.UserId,
                ShowtimeId = createDto.ShowtimeId,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow,
                Status = ReservationStatus.Confirmed,
                ReservationSeats = new List<ReservationSeat>()
            };

            // Adicionar os assentos à reserva
            foreach (var seat in seats)
            {
                reservation.ReservationSeats.Add(new ReservationSeat
                {
                    ReservationId = reservation.Id,
                    SeatId = seat.Id
                });

                // Marcar o assento como reservado
                seat.IsReserved = true;
                await _unitOfWork.Seats.UpdateAsync(seat);
            }

            await _unitOfWork.Reservations.AddAsync(reservation);
            await _unitOfWork.CompleteAsync();

            // Buscar a reserva completa com todas as relações
            var createdReservation = await _unitOfWork.Reservations.GetByIdAsync(reservation.Id);
            return _mapper.Map<ReservationDto>(createdReservation);
        }

        public async Task<ReservationDto> CancelAsync(Guid id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
                throw new ApplicationException("Reservation not found");

            // Verificar se a reserva já não está cancelada
            if (reservation.Status == ReservationStatus.Cancelled)
                throw new ApplicationException("Reservation is already cancelled");

            // Atualizar o status da reserva
            reservation.Status = ReservationStatus.Cancelled;

            // Liberar os assentos reservados
            foreach (var reservationSeat in reservation.ReservationSeats)
            {
                var seat = await _unitOfWork.Seats.GetByIdAsync(reservationSeat.SeatId);
                if (seat != null)
                {
                    seat.IsReserved = false;
                    await _unitOfWork.Seats.UpdateAsync(seat);
                }
            }

            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
                throw new ApplicationException("Reservation not found");

            // Liberar os assentos reservados se a reserva não estiver cancelada
            if (reservation.Status != ReservationStatus.Cancelled)
            {
                foreach (var reservationSeat in reservation.ReservationSeats)
                {
                    var seat = await _unitOfWork.Seats.GetByIdAsync(reservationSeat.SeatId);
                    if (seat != null)
                    {
                        seat.IsReserved = false;
                        await _unitOfWork.Seats.UpdateAsync(seat);
                    }
                }
            }

            await _unitOfWork.Reservations.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
