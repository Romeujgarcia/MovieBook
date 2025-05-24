using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieBookingSystem.Infrastructure.Identity;
using AppException = MovieBookingSystem.Application.Common.Exceptions.ApplicationException;

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

            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(registerDto.Password);

            // Add user to repository
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> UpdateAsync(Guid userId, UpdateUserDto updateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
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
                if (existingUser != null && existingUser.Id != userId)
                    throw new ApplicationException("Email already registered by another user");

                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.FullName))
                user.FullName = updateDto.FullName;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApplicationException("User not found");

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<LoginResponseDto> AuthenticateAsync(LoginUserDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new ApplicationException("Invalid email or password");

            return new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                // Outros dados que desejar retornar
            };
        }
    }
}

