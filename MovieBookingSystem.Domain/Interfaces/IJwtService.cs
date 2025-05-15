using System;
using System.Collections.Generic;
using System.Security.Claims;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<string> roles = null);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
