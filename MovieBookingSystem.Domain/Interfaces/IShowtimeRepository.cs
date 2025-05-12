using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IShowtimeRepository
    {
        Task<Showtime> GetByIdAsync(Guid id);
        Task<IEnumerable<Showtime>> GetAllAsync();
        Task<IEnumerable<Showtime>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Showtime>> GetByMovieIdAsync(Guid movieId);
        Task<IEnumerable<Showtime>> GetByMovieIdAndDateAsync(Guid movieId, DateTime date);
        Task<Showtime> AddAsync(Showtime showtime);
        Task UpdateAsync(Showtime showtime);
        Task DeleteAsync(Guid id);
    }
}
