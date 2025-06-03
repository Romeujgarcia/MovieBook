using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<IList<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(Guid id);
        Task<Genre> AddAsync(Genre genre);
        Task<Genre> UpdateAsync(Genre genre);
        Task DeleteAsync(Genre genre);
    }
}