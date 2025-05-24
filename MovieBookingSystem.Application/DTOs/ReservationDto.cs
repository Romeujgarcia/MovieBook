using MovieBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Application.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid ShowtimeId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ShowtimeStart { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class CreateReservationDto
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public List<Guid> SeatIds { get; set; } = new List<Guid>();
    }
}
