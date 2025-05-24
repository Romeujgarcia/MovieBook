using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto registerDto);
        Task<UserDto> GetByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<LoginResponseDto> AuthenticateAsync(LoginUserDto loginDto);
        Task<UserDto> UpdateAsync(Guid userId, UpdateUserDto updateDto);
        Task DeleteAsync(Guid userId);
    }
}
