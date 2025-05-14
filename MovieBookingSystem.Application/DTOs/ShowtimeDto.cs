using System;

namespace MovieBookingSystem.Application.DTOs
{
    public class ShowtimeDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string Theater { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
    }

    public class CreateShowtimeDto
    {
        public Guid MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string Theater { get; set; }
        public int TotalRows { get; set; }
        public int SeatsPerRow { get; set; }
    }

    public class UpdateShowtimeDto
    {
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string Theater { get; set; }
    }
}
