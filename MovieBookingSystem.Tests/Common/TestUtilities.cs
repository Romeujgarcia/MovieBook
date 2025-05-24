using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.Api.Models;
using System;
using System.Security.Claims;

namespace MovieBookingSystem.Tests.Common
{
    public static class TestUtilities
    {
        public static T? GetObjectResultContent<T>(ActionResult<T> result)
        {
            if (result.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value is ApiResponse apiResponse)
                {
                    return (T?)apiResponse.Data;
                }
                return (T?)okObjectResult.Value;
            }
            
            return default;
        }

        public static HttpContext CreateHttpContext(Guid? userId = null, bool isAdmin = false)
        {
            var role = isAdmin ? "Admin" : "User";
            
            // Create a new HttpContext with a ClaimsPrincipal
            return CreateHttpContext(userId, role);
        }

        private static HttpContext CreateHttpContext(Guid? userId, string role)
        {
            var httpContext = new DefaultHttpContext();
            if (userId.HasValue)
            {
                httpContext.Items["UserId"] = userId.Value;
                
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };
                
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                httpContext.User = principal;
            }
            
            return httpContext;
        }
    }
}
