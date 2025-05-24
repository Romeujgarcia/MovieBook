using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<IList<Seat>> GetAllAsync();
        Task<Seat> GetByIdAsync(Guid id);
        Task<IList<Seat>> GetByShowtimeIdAsync(Guid showtimeId);
        Task<IList<Seat>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId);
        Task<IList<Seat>> GetByIdsAsync(IList<Guid> ids);
        Task<bool> IsSeatAvailableAsync(Guid seatId);
        Task<Seat> AddAsync(Seat seat);
        Task<Seat> UpdateAsync(Seat seat);
        Task DeleteAsync(Seat seat);
    }
}
