using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Api.Models; // Usando seu ApiResponse existente
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieBookingSystem.Application.Common.Exceptions;

namespace MovieBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating reservation for user {UserId}, showtime {ShowtimeId}", 
                    createDto?.UserId, createDto?.ShowtimeId);

                if (createDto == null)
                {
                    _logger.LogWarning("CreateReservationDto is null");
                    return BadRequest(ApiResponse.ErrorResponse("Request body is required"));
                }

                // Log the incoming data for debugging
                _logger.LogDebug("Reservation request: UserId={UserId}, ShowtimeId={ShowtimeId}, SeatCount={SeatCount}", 
                    createDto.UserId, createDto.ShowtimeId, createDto.SeatIds?.Count ?? 0);

                var reservation = await _reservationService.CreateReservationAsync(createDto, createDto.UserId);
                
                _logger.LogInformation("Reservation created successfully with ID {ReservationId}", reservation.Id);

                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, 
                    ApiResponse.SuccessResponse("Reservation created successfully", reservation));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
                return NotFound(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument: {Message}", ex.Message);
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (MovieBookingSystem.Application.Common.Exceptions.ApplicationException ex)
            {
                _logger.LogWarning(ex, "Application error: {Message}", ex.Message);
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating reservation: {Message}", ex.Message);
                return StatusCode(500, ApiResponse.ErrorResponse("An error occurred while processing your request."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(Guid id)
        {
            try
            {
                _logger.LogInformation("Getting reservation {ReservationId}", id);
                
                var reservation = await _reservationService.GetByIdAsync(id);
                
                return Ok(ApiResponse.SuccessResponse("Reservation retrieved successfully", reservation));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Reservation not found: {ReservationId}", id);
                return NotFound(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservation {ReservationId}: {Message}", id, ex.Message);
                return StatusCode(500, ApiResponse.ErrorResponse("An error occurred while processing your request."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            try
            {
                _logger.LogInformation("Cancelling reservation {ReservationId}", id);
                
                var reservation = await _reservationService.CancelReservationAsync(id);
                
                return Ok(ApiResponse.SuccessResponse("Reservation cancelled successfully", reservation));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Reservation not found for cancellation: {ReservationId}", id);
                return NotFound(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (MovieBookingSystem.Application.Common.Exceptions.ApplicationException ex)
            {
                _logger.LogWarning(ex, "Cannot cancel reservation {ReservationId}: {Message}", id, ex.Message);
                return BadRequest(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling reservation {ReservationId}: {Message}", id, ex.Message);
                return StatusCode(500, ApiResponse.ErrorResponse("An error occurred while processing your request."));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReservations(Guid userId)
        {
            try
            {
                _logger.LogInformation("Getting reservations for user {UserId}", userId);
                
                var reservations = await _reservationService.GetUserReservationsAsync(userId);
                
                return Ok(ApiResponse.SuccessResponse("User reservations retrieved successfully", reservations));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found: {UserId}", userId);
                return NotFound(ApiResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservations for user {UserId}: {Message}", userId, ex.Message);
                return StatusCode(500, ApiResponse.ErrorResponse("An error occurred while processing your request."));
            }
        }
    }
}