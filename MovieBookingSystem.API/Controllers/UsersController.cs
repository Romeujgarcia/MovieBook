using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Api.Models;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Infrastructure.Services; // Adicione esta linha
using MovieBookingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace MovieBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
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
            // Obter o UserDto do serviço
            var result = await _userService.LoginAsync(loginDto.Email, loginDto.Password);

            // Criar um User para passar para o gerador de token
            var user = new User
            {
                Id = result.Id, // Accessing the Id directly from result
                UserName = result.UserName,
                Email = result.Email,
                IsAdmin = result.IsAdmin, // Ensure this property exists in UserDto
                FullName = result.FullName,
                CreatedAt = result.CreatedAt
            };

            var token = _jwtService.GenerateToken(user, result.Roles); // Use the user object created above

            // Create the response DTO
            var response = new LoginResponseDto
            {
                User = result, // Assign the UserDto
                Token = token // Assign the generated token
            };

            return Ok(ApiResponse.SuccessResponse("Login successful", response));
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
