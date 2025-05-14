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
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        /// <summary>
        /// Obtém todos os gêneros
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(ApiResponse.SuccessResponse("Genres retrieved successfully", genres));
        }

        /// <summary>
        /// Obtém um gênero pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            return Ok(ApiResponse.SuccessResponse("Genre retrieved successfully", genre));
        }

        /// <summary>
        /// Cria um novo gênero (apenas para administradores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> Create([FromBody] CreateGenreDto createDto)
        {
            var genre = await _genreService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, 
                ApiResponse.SuccessResponse("Genre created successfully", genre));
        }

        /// <summary>
        /// Atualiza um gênero existente (apenas para administradores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreDto updateDto)
        {
            var genre = await _genreService.UpdateAsync(id, updateDto);
            return Ok(ApiResponse.SuccessResponse("Genre updated successfully", genre));
        }

        /// <summary>
        /// Exclui um gênero (apenas para administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _genreService.DeleteAsync(id);
            return Ok(ApiResponse.SuccessResponse("Genre deleted successfully"));
        }
    }
}
