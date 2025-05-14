using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<ReservationDto>> GetByShowtimeIdAsync(Guid showtimeId);
        Task<ReservationDto> CreateAsync(CreateReservationDto createDto);
        Task<ReservationDto> CancelAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}