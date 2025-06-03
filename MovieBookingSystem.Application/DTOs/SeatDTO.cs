using System;

namespace MovieBookingSystem.Application.DTOs
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public Guid ShowtimeId { get; set; }
        public string Row { get; set; }
        public int SeatNumber { get; set; }
        public bool IsReserved { get; set; }
    }
}