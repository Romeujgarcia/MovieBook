using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid ShowtimeId { get; set; }
        public string Row { get; set; } // A, B, C, ...
        public int SeatNumber { get; set; }   // 1, 2, 3, ...
        public bool IsReserved { get; set; }
        public bool IsAvailable { get; set; } = true; // Add this property with default value
        public SeatType Type { get; set; } = SeatType.Regular; // Add this property

        // Relacionamentos
        public Showtime Showtime { get; set; }
        public ICollection<ReservationSeat> ReservationSeats { get; set; }
    }
   
}
