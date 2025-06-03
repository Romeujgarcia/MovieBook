using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieBookingSystem.Api.Models;
using MovieBookingSystem.Application.Common.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationException = MovieBookingSystem.Application.Common.Exceptions.ApplicationException;

namespace MovieBookingSystem.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ApiResponse response;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = "Validation error",
                        ValidationErrors = validationException.Errors
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = notFoundException.Message
                    };
                    break;

                case ApplicationException appException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = appException.Message
                    };
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = "Unauthorized access"
                    };
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception");
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = "An error occurred while processing your request."
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
} 