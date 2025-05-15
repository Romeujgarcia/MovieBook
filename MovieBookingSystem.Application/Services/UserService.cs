using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto registerDto)
        {
            // Check if email or username already exists
            var existingByEmail = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingByEmail != null)
                throw new ApplicationException("Email already registered");

            var existingByUsername = await _unitOfWork.Users.GetByUsernameAsync(registerDto.UserName);
            if (existingByUsername != null)
                throw new ApplicationException("Username already taken");

            // Create new user entity
            var user = _mapper.Map<User>(registerDto);
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;

             // Set IsAdmin based on the DTO
            //user.IsAdmin = registerDto.IsAdmin; // Define se é admin com base no DTO

            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(registerDto.Password);

            // Add user to repository
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new ApplicationException("User not found");

            // Validate current password if provided
            if (!string.IsNullOrEmpty(updateDto.CurrentPassword))
            {
                bool validPassword = _passwordHasher.VerifyPassword(updateDto.CurrentPassword, user.PasswordHash);
                if (!validPassword)
                    throw new ApplicationException("Invalid current password");

                // Update password if new password is provided
                if (!string.IsNullOrEmpty(updateDto.NewPassword))
                {
                    user.PasswordHash = _passwordHasher.HashPassword(updateDto.NewPassword);
                }
            }

            // Update other properties
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                // Check if email is already used by another user
                var existingUser = await _unitOfWork.Users.GetByEmailAsync(updateDto.Email);
                if (existingUser != null && existingUser.Id != id)
                    throw new ApplicationException("Email already registered by another user");

                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.FullName))
                user.FullName = updateDto.FullName;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null) 
                return false;

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }


        public async Task<IEnumerable<ShowtimeDto>> GetByMovieAndDateAsync(Guid movieId, DateTime date)
        {
            var showtimes = await _unitOfWork.Showtimes.GetByMovieIdAndDateAsync(movieId, date);
            return _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);
        }

        public async Task<UserDto> LoginAsync(string email, string password)
        {
            // Changed from username to email to match the interface
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))// Use _passwordHasher
                return null;

            return _mapper.Map<UserDto>(user);
        }

    }
}   
 