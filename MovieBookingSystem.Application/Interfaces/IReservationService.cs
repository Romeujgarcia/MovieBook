using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto, Guid userId);
        Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(Guid userId);
        Task<ReservationDto> GetByIdAsync(Guid reservationId);
        Task<ReservationDto> CancelReservationAsync(Guid reservationId);
        Task DeleteAsync(Guid reservationId);
    }
}