using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieDto>> GetAllAsync();
        Task<MovieDto> GetByIdAsync(Guid id);
        Task<List<MovieDto>> GetByGenreAsync(Guid genreId);
        Task<List<MovieDto>> SearchAsync(string term);
        Task<MovieDto> AddAsync(CreateMovieDto createMovieDto);
        Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto updateMovieDto);
        Task DeleteAsync(Guid id);
    }
}