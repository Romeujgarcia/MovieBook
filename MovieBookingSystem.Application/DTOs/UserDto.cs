using System;

namespace MovieBookingSystem.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } // Add this line if you need IsAdmin
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
       

        
    }

    public class LoginResponseDto
    {
        public UserDto User { get; set; } // Include the user information
        public string Token { get; set; } // Include the token
    }
}
