using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Relacionamentos
        public User User { get; set; }
        public Showtime Showtime { get; set; }
        public ICollection<ReservationSeat> ReservationSeats { get; set; }
    }
    
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
