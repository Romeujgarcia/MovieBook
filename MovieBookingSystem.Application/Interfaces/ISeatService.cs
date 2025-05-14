using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface ISeatService
    {
        Task<SeatDto> GetByIdAsync(Guid id);
        Task<IEnumerable<SeatDto>> GetByShowtimeIdAsync(Guid showtimeId);
        Task<IEnumerable<SeatDto>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId);
    }
}