using MovieBookingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> RegisterAsync(RegisterUserDto registerDto);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<UserDto> LoginAsync(string email, string password);
    }
}
