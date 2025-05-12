using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre> GetByIdAsync(Guid id);
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(Guid id);
    }
}
