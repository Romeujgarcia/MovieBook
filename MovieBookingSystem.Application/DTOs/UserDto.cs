using System;

namespace MovieBookingSystem.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; } // Adicionada
        public string NewPassword { get; set; } // Adicionada
        public string FullName { get; set; }
    }

   public class LoginResponseDto
    {
        public Guid UserId { get; set; } // Adicionada
        public string Email { get; set; } // Adicionada
        public string FullName { get; set; } // Adicionada
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
