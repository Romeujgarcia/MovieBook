using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat> GetByIdAsync(Guid id);
        Task<IEnumerable<Seat>> GetAllAsync();
        Task<IEnumerable<Seat>> GetByShowtimeIdAsync(Guid showtimeId);
        Task<IEnumerable<Seat>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId);
        Task<Seat> AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(Guid id);
        Task<bool> ReserveSeatAsync(Guid seatId);
        Task<bool> ReleaseSeatAsync(Guid seatId);
    }
}
