using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Reservation>> GetByShowtimeIdAsync(Guid showtimeId);
        Task<IEnumerable<Reservation>> GetActiveReservationsByUserIdAsync(Guid userId);
        Task<Reservation> AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Guid id);
    }
}
