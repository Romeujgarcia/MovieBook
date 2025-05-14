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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            return Ok(ApiResponse.SuccessResponse("User registered successfully", user));
        }

        /// <summary>
        /// Faz login de um usuário
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            // Verifique se o loginDto não é nulo
            if (loginDto == null)
            {
                return BadRequest(ApiResponse.ErrorResponse("Invalid login request"));
            }
             // Chame o método LoginAsync com email e password
            var result = await _userService.LoginAsync(loginDto.Email, loginDto.Password); 
            if (result == null)
            {
                return Unauthorized(ApiResponse.ErrorResponse("Invalid email or password"));
            }
            return Ok(ApiResponse.SuccessResponse("Login successful", result));
        }

        /// <summary>
        /// Obtém o perfil do usuário autenticado
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var user = await _userService.GetByIdAsync(userId);
            return Ok(ApiResponse.SuccessResponse("User profile retrieved successfully", user));
        }

        /// <summary>
        /// Atualiza o perfil do usuário autenticado
        /// </summary>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateDto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var user = await _userService.UpdateAsync(userId, updateDto);
            return Ok(ApiResponse.SuccessResponse("User profile updated successfully", user));
        }

        /// <summary>
        /// Obtém um usuário pelo ID (apenas para administradores)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(ApiResponse.SuccessResponse("User retrieved successfully", user));
        }

        /// <summary>
        /// Obtém todos os usuários (apenas para administradores)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(ApiResponse.SuccessResponse("Users retrieved successfully", users));
        }

        /// <summary>
        /// Exclui um usuário (apenas para administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok(ApiResponse.SuccessResponse("User deleted successfully"));
        }
    }
}
