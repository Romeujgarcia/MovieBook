using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Api.Models;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace MovieBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Obtém uma reserva pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var reservation = await _reservationService.GetByIdAsync(id);
            
            // Verificar se a reserva pertence ao usuário atual (a menos que seja admin)
            if (reservation.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            
            return Ok(ApiResponse.SuccessResponse("Reservation retrieved successfully", reservation));
        }

        /// <summary>
        /// Obtém todas as reservas do usuário autenticado
        /// </summary>
        [HttpGet("my-reservations")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var reservations = await _reservationService.GetByUserIdAsync(userId);
            return Ok(ApiResponse.SuccessResponse("Reservations retrieved successfully", reservations));
        }

        /// <summary>
        /// Obtém todas as reservas de uma sessão (apenas para administradores)
        /// </summary>
        [HttpGet("by-showtime/{showtimeId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> GetByShowtime(Guid showtimeId)
        {
            var reservations = await _reservationService.GetByShowtimeIdAsync(showtimeId);
            return Ok(ApiResponse.SuccessResponse("Reservations retrieved successfully", reservations));
        }

        /// <summary>
        /// Cria uma nova reserva
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto createDto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            
            // Garantir que o usuário só possa fazer reservas para si mesmo (a menos que seja admin)
            if (createDto.UserId != userId && !User.IsInRole("Admin"))
            {
                createDto.UserId = userId; // Forçar que a reserva seja para o usuário autenticado
            }
            
            var reservation = await _reservationService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, 
                ApiResponse.SuccessResponse("Reservation created successfully", reservation));
        }

        /// <summary>
        /// Cancela uma reserva
        /// </summary>
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var reservation = await _reservationService.GetByIdAsync(id);
            
            // Verificar se a reserva pertence ao usuário atual (a menos que seja admin)
            if (reservation.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            
            var cancelledReservation = await _reservationService.CancelAsync(id);
            return Ok(ApiResponse.SuccessResponse("Reservation cancelled successfully", cancelledReservation));
        }

        /// <summary>
        /// Exclui uma reserva (apenas para administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reservationService.DeleteAsync(id);
            return Ok(ApiResponse.SuccessResponse("Reservation deleted successfully"));
        }
    }
}
