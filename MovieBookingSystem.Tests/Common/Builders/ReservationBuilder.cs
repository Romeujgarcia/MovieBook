using System;
using System.Collections.Generic;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Tests.Common.Builders
{
    public class ReservationBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private Guid _showtimeId = Guid.NewGuid();
        private DateTime _reservationDate = DateTime.UtcNow;
        private decimal _totalPrice = 20.99m;
        private ReservationStatus _status = ReservationStatus.Confirmed;
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt = null;
        private List<Guid> _seatIds = new List<Guid>();

        public ReservationBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ReservationBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public ReservationBuilder WithShowtimeId(Guid showtimeId)
        {
            _showtimeId = showtimeId;
            return this;
        }

        public ReservationBuilder WithReservationDate(DateTime reservationDate)
        {
            _reservationDate = reservationDate;
            return this;
        }

        public ReservationBuilder WithTotalPrice(decimal totalPrice)
        {
            _totalPrice = totalPrice;
            return this;
        }

        public ReservationBuilder WithStatus(ReservationStatus status)
        {
            _status = status;
            return this;
        }

        public ReservationBuilder WithCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public ReservationBuilder WithUpdatedAt(DateTime? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public ReservationBuilder WithSeat(Guid seatId)
        {
            _seatIds.Add(seatId);
            return this;
        }

        public Reservation Build()
        {
            var reservation = new Reservation
            {
                Id = _id,
                UserId = _userId,
                ShowtimeId = _showtimeId,
                ReservationDate = _reservationDate,
                TotalPrice = _totalPrice,
                Status = _status,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt,
                ReservationSeats = new List<ReservationSeat>()
            };

            foreach (var seatId in _seatIds)
            {
                reservation.ReservationSeats.Add(new ReservationSeat
                {
                    ReservationId = _id,
                    SeatId = seatId
                });
            }

            return reservation;
        }
    }
}