using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<IList<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(Guid id);
        Task<IList<Reservation>> GetByUserIdAsync(Guid userId);
        Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation> UpdateAsync(Reservation reservation);
        Task DeleteAsync(Guid reservationId);
    }
}
