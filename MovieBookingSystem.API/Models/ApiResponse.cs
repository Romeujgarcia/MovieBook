using System.Collections.Generic;

namespace MovieBookingSystem.Api.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        public static ApiResponse SuccessResponse(string message = "Request successful", object data = null)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse ErrorResponse(string message = "Request failed", object data = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }

    // Versão genérica para uso em testes e casos específicos
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        public static ApiResponse<T> SuccessResponse(string message = "Request successful", T data = default(T))
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message = "Request failed", T data = default(T))
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
}