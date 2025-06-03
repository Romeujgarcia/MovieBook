using System;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Tests.Common.Builders
{
    public class SeatBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _showtimeId = Guid.NewGuid();
        private string _row = "A";
        private int _seatNumber = 1;
        private bool _isReserved = false;
        private bool _isAvailable = true;
        private SeatType _type = SeatType.Regular;

        public SeatBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SeatBuilder WithShowtimeId(Guid showtimeId)
        {
            _showtimeId = showtimeId;
            return this;
        }

        public SeatBuilder WithRow(string row)
        {
            _row = row;
            return this;
        }

        public SeatBuilder WithSeatNumber(int seatNumber)
        {
            _seatNumber = seatNumber;
            return this;
        }

        public SeatBuilder WithIsReserved(bool isReserved)
        {
            _isReserved = isReserved;
            return this;
        }

        public SeatBuilder WithIsAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;
            return this;
        }

        public SeatBuilder WithType(SeatType type)
        {
            _type = type;
            return this;
        }

        public Seat Build()
        {
            return new Seat
            {
                Id = _id,
                ShowtimeId = _showtimeId,
                Row = _row,
                SeatNumber = _seatNumber,
                IsReserved = _isReserved,
                IsAvailable = _isAvailable,
                Type = _type
            };
        }
    }
}
