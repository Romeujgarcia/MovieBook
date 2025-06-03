using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MovieBookingSystem.Application.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDto> GetByIdAsync(Guid id);
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto> CreateAsync(CreateGenreDto createDto);
        Task<GenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto);
        Task DeleteAsync(Guid id);
    }
}
