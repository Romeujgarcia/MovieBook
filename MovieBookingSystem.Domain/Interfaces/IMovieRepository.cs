using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<IList<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(Guid id);
        Task<IList<Movie>> GetByGenreAsync(Guid genreId);
        Task<IList<Movie>> SearchAsync(string term);
        Task<Movie> AddAsync(Movie movie);
        Task<Movie> UpdateAsync(Movie movie);
        Task DeleteAsync(Movie movie);
    }
}
