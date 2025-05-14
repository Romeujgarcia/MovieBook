using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IShowtimeService
    {
        Task<ShowtimeDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ShowtimeDto>> GetAllAsync();
        Task<IEnumerable<ShowtimeDto>> GetByDateAsync(DateTime date);
        Task<IEnumerable<ShowtimeDto>> GetByMovieIdAsync(Guid movieId);
        Task<IEnumerable<ShowtimeDto>> GetByMovieIdAndDateAsync(Guid movieId, DateTime date);
        Task<ShowtimeDto> CreateAsync(CreateShowtimeDto createDto);
        Task<ShowtimeDto> UpdateAsync(Guid id, UpdateShowtimeDto updateDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ShowtimeDto>> GetByMovieAndDateAsync(Guid movieId, DateTime date);
    }
}