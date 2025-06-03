
using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class Showtime
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } // Add this property
        public string Hall { get; set; } // Add this property
        public decimal Price { get; set; }
        public decimal TicketPrice { get; set; } // Add this property for backward compatibility
        public int TotalSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Relacionamentos
        public Movie Movie { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
