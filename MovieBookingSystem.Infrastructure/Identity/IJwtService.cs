using System;
using System.Collections.Generic;
using System.Security.Claims;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Infrastructure.Identity
{
    public interface IJwtService
    {
        string GenerateToken(User user, bool IsAdmin);
        bool ValidateToken(string token);
    }
}
