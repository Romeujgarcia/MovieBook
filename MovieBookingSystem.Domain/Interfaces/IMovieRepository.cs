using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> GetByIdAsync(Guid id);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Movie>> GetByGenreAsync(Guid genreId);
        Task<Movie> AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(Guid id);
    }
}