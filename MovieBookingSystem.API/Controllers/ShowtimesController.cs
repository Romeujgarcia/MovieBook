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
    public class ShowtimesController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;

        public ShowtimesController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        /// <summary>
        /// Obtém todas as sessões
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null)
        {
            var showtimes = date.HasValue 
                ? await _showtimeService.GetByDateAsync(date.Value)
                : await _showtimeService.GetAllAsync();

            return Ok(ApiResponse.SuccessResponse("Showtimes retrieved successfully", showtimes));
        }

        /// <summary>
        /// Obtém uma sessão pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);
            return Ok(ApiResponse.SuccessResponse("Showtime retrieved successfully", showtime));
        }

        /// <summary>
        /// Obtém sessões por filme
        /// </summary>
        [HttpGet("by-movie/{movieId}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetByMovie(Guid movieId, [FromQuery] DateTime? date = null)
        {
            var showtimes = date.HasValue
                ? await _showtimeService.GetByMovieAndDateAsync(movieId, date.Value)
                : await _showtimeService.GetByMovieIdAsync(movieId);

            return Ok(ApiResponse.SuccessResponse("Showtimes by movie retrieved successfully", showtimes));
        }

        /// <summary>
        /// Cria uma nova sessão (apenas para administradores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> Create([FromBody] CreateShowtimeDto createDto)
        {
            var showtime = await _showtimeService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = showtime.Id }, 
                ApiResponse.SuccessResponse("Showtime created successfully", showtime));
        }

        /// <summary>
        /// Atualiza uma sessão existente (apenas para administradores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShowtimeDto updateDto)
        {
            var showtime = await _showtimeService.UpdateAsync(id, updateDto);
            return Ok(ApiResponse.SuccessResponse("Showtime updated successfully", showtime));
        }

        /// <summary>
        /// Exclui uma sessão (apenas para administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _showtimeService.DeleteAsync(id);
            return Ok(ApiResponse.SuccessResponse("Showtime deleted successfully"));
        }
    }
}
