using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShowtimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ShowtimeDto> GetByIdAsync(Guid id)
        {
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(id);
            if (showtime == null)
                return null;

            var result = _mapper.Map<ShowtimeDto>(showtime);

            // Get seat counts - Correção aqui: usar Count() em vez de Count
            var seats = await _unitOfWork.Seats.GetByShowtimeIdAsync(id);
            result.TotalSeats = seats != null ? seats.Count() : 0;

            var availableSeats = await _unitOfWork.Seats.GetAvailableSeatsByShowtimeIdAsync(id);
            result.AvailableSeats = availableSeats != null ? availableSeats.Count() : 0;

            return result;
        }

        public async Task<IEnumerable<ShowtimeDto>> GetAllAsync()
        {
            var showtimes = await _unitOfWork.Showtimes.GetAllAsync();
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }

        public async Task<IEnumerable<ShowtimeDto>> GetByDateAsync(DateTime date)
        {
            var showtimes = await _unitOfWork.Showtimes.GetByDateAsync(date);
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }

        public async Task<IEnumerable<ShowtimeDto>> GetByMovieIdAsync(Guid movieId)
        {
            var showtimes = await _unitOfWork.Showtimes.GetByMovieIdAsync(movieId);
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }

        public async Task<IEnumerable<ShowtimeDto>> GetByMovieIdAndDateAsync(Guid movieId, DateTime date)
        {
            var showtimes = await _unitOfWork.Showtimes.GetByMovieIdAndDateAsync(movieId, date);
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }
       public async Task<ShowtimeDto> CreateAsync(CreateShowtimeDto createDto)
{
    // Validate input
    if (createDto.MovieId == Guid.Empty)
        throw new ArgumentException("MovieId cannot be empty", nameof(createDto.MovieId));

    // Validate movie exists with more detailed error information
    var movie = await _unitOfWork.Movies.GetByIdAsync(createDto.MovieId);
    if (movie == null)
    {
        // Get all available movies for better error message
        var availableMovies = await _unitOfWork.Movies.GetAllAsync();
        var availableMovieInfo = availableMovies.Select(m => new { m.Id, m.Title }).ToList();
        
        var errorMessage = $"Movie with ID '{createDto.MovieId}' not found. ";
        if (availableMovieInfo.Any())
        {
            errorMessage += $"Available movies: {string.Join(", ", availableMovieInfo.Select(m => $"{m.Title} ({m.Id})"))}";
        }
        else
        {
            errorMessage += "No movies are currently available in the system.";
        }
        
        throw new NotFoundException(errorMessage, createDto.MovieId);
    }

    // Validate showtime doesn't conflict with existing ones
    var existingShowtimes = await _unitOfWork.Showtimes.GetByMovieIdAndDateAsync(createDto.MovieId, createDto.StartTime.Date);
    var conflictingShowtime = existingShowtimes.FirstOrDefault(s => 
        s.Hall == createDto.Theater && 
        Math.Abs((s.StartTime - createDto.StartTime).TotalMinutes) < movie.DurationMinutes + 30); // 30 min buffer

    if (conflictingShowtime != null)
    {
        throw new InvalidOperationException(
            $"Showtime conflicts with existing showtime in {createDto.Theater} at {conflictingShowtime.StartTime:yyyy-MM-dd HH:mm}. " +
            $"Please choose a different time or theater.");
    }

    // Create showtime
    var showtime = _mapper.Map<Showtime>(createDto);
    showtime.Id = Guid.NewGuid();

    await _unitOfWork.Showtimes.AddAsync(showtime);
    await _unitOfWork.CompleteAsync();

    // Create seats for this showtime
    await CreateSeatsForShowtime(showtime.Id, createDto.TotalRows, createDto.SeatsPerRow);

    // Get complete showtime with seats
    var createdShowtime = await _unitOfWork.Showtimes.GetByIdAsync(showtime.Id);
    var result = _mapper.Map<ShowtimeDto>(createdShowtime);

    // Get seat counts
    var seats = await _unitOfWork.Seats.GetByShowtimeIdAsync(showtime.Id);
    result.TotalSeats = seats != null ? seats.Count() : 0;
    result.AvailableSeats = result.TotalSeats;  // All seats are available initially

    return result;
}

        private async Task CreateSeatsForShowtime(Guid showtimeId, int totalRows, int seatsPerRow)
        {
            string[] rowLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            for (int row = 0; row < totalRows; row++)
            {
                string rowLetter = rowLetters[row];

                for (int seatNum = 1; seatNum <= seatsPerRow; seatNum++)
                {
                    var seat = new Seat
                    {
                        Id = Guid.NewGuid(),
                        ShowtimeId = showtimeId,
                        Row = rowLetter,
                        SeatNumber = seatNum,
                        IsReserved = false
                    };

                    await _unitOfWork.Seats.AddAsync(seat);
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<ShowtimeDto> UpdateAsync(Guid id, UpdateShowtimeDto updateDto)
        {
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(id);
            if (showtime == null)
                throw new NotFoundException("Showtime", id);

            _mapper.Map(updateDto, showtime);
            await _unitOfWork.Showtimes.UpdateAsync(showtime);
            await _unitOfWork.CompleteAsync();

            // Obter o showtime atualizado com os assentos
            var updatedShowtime = await _unitOfWork.Showtimes.GetByIdAsync(id);
            var result = _mapper.Map<ShowtimeDto>(updatedShowtime);

            // Obter contagens de assentos
            var seats = await _unitOfWork.Seats.GetByShowtimeIdAsync(showtime.Id);
            result.TotalSeats = seats != null ? seats.Count() : 0;

            var availableSeats = await _unitOfWork.Seats.GetAvailableSeatsByShowtimeIdAsync(showtime.Id);
            result.AvailableSeats = availableSeats != null ? availableSeats.Count() : 0;

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Showtimes.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ShowtimeDto>> GetByMovieAndDateAsync(Guid movieId, DateTime date)
        {
            var showtimes = await _unitOfWork.Showtimes.GetByMovieIdAndDateAsync(movieId, date);
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }
    }
}

