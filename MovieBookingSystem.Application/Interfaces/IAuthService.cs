
using System.Threading.Tasks;
using MovieBookingSystem.Application.DTOs;

namespace MovieBookingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string email, string password);
        // Potencialmente outros métodos relacionados a auth: RefreshToken, LogoutAsync, etc.
    }
}
