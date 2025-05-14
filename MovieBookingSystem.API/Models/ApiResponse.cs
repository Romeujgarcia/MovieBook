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
}
