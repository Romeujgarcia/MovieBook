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
    public DateTime EndTime { get; set; }
    
    // Ensure this property exists
    public string Hall { get; set; } 
    
    // Add the missing properties
    public string Theater { get; set; } // Add this property
    public int TotalRows { get; set; } // Add this property
    public int SeatsPerRow { get; set; } // Add this property
    
    public decimal Price { get; set; }
    public decimal TicketPrice { get; set; }
    //public bool IsActive { get; set; }
}

   

    public class UpdateShowtimeDto
    {
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string Theater { get; set; }
    }
}
