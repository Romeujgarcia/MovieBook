
using System;
using System.Threading.Tasks;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Identity;
using MovieBookingSystem.Application.Interfaces;

namespace MovieBookingSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                FullName = user.FullName,
                //CreatedAt = user.CreatedAt
            };

            var token = _jwtService.GenerateToken(user, user.IsAdmin);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Token = token
            };
        }
    }


}
