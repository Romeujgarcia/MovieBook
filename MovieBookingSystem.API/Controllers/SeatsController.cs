using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Api.Models;
using MovieBookingSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace MovieBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Obtém um assento pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var seat = await _seatService.GetByIdAsync(id);
            return Ok(ApiResponse.SuccessResponse("Seat retrieved successfully", seat));
        }

        /// <summary>
        /// Obtém todos os assentos de uma sessão
        /// </summary>
        [HttpGet("by-showtime/{showtimeId}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetByShowtime(Guid showtimeId)
        {
            var seats = await _seatService.GetByShowtimeIdAsync(showtimeId);
            return Ok(ApiResponse.SuccessResponse("Seats retrieved successfully", seats));
        }

        /// <summary>
        /// Obtém assentos disponíveis de uma sessão
        /// </summary>
        [HttpGet("available/by-showtime/{showtimeId}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetAvailableByShowtime(Guid showtimeId)
        {
            var seats = await _seatService.GetAvailableSeatsByShowtimeIdAsync(showtimeId);
            return Ok(ApiResponse.SuccessResponse("Available seats retrieved successfully", seats));
        }
    }
}
