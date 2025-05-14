using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<IEnumerable<MovieDto>> GetByGenreAsync(Guid genreId);
        Task<MovieDto> CreateAsync(CreateMovieDto createDto);
        Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto updateDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<MovieDto>> GetByGenreIdAsync(Guid genreId);
        Task<IEnumerable<MovieDto>> SearchByTitleAsync(string title);
    }
}
