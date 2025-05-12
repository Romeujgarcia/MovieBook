using System;

namespace MovieBookingSystem.Domain.Entities
{
    // Tabela de junção entre Reservation e Seat (muitos para muitos)
    public class ReservationSeat
    {
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; }
    }
}
