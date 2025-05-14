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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Obtém todos os filmes
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(ApiResponse.SuccessResponse("Movies retrieved successfully", movies));
        }

        /// <summary>
        /// Obtém um filme pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            return Ok(ApiResponse.SuccessResponse("Movie retrieved successfully", movie));
        }

        /// <summary>
        /// Cria um novo filme (apenas para administradores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> Create([FromBody] CreateMovieDto createDto)
        {
            var movie = await _movieService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, 
                ApiResponse.SuccessResponse("Movie created successfully", movie));
        }

        /// <summary>
        /// Atualiza um filme existente (apenas para administradores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieDto updateDto)
        {
            var movie = await _movieService.UpdateAsync(id, updateDto);
            return Ok(ApiResponse.SuccessResponse("Movie updated successfully", movie));
        }

        /// <summary>
        /// Exclui um filme (apenas para administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _movieService.DeleteAsync(id);
            return Ok(ApiResponse.SuccessResponse("Movie deleted successfully"));
        }

        /// <summary>
        /// Pesquisa filmes por título
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var movies = await _movieService.SearchByTitleAsync(title);
            return Ok(ApiResponse.SuccessResponse("Movies search completed", movies));
        }

        /// <summary>
        /// Obtém filmes por gênero
        /// </summary>
        [HttpGet("by-genre/{genreId}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetByGenre(Guid genreId)
        {
            var movies = await _movieService.GetByGenreIdAsync(genreId);
            return Ok(ApiResponse.SuccessResponse("Movies by genre retrieved successfully", movies));
        }
    }
}
