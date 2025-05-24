using System;

namespace MovieBookingSystem.Domain.Entities
{
    public class ReservationSeat
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid SeatId { get; set; }
        
        // Relationships
        public Reservation Reservation { get; set; }
        public Seat Seat { get; set; }
    }
}
