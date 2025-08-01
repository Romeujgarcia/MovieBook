using System.Collections.Generic;

namespace MovieBookingSystem.Application.DTOs
{
    // DTOs for deserialization
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string[]> ValidationErrors { get; set; }
    }
}