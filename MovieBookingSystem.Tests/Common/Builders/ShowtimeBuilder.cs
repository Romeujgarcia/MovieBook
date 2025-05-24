using System;
using System.Collections.Generic;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Tests.Common.Builders
{
    public class ShowtimeBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _movieId = Guid.NewGuid();
        private DateTime _startTime = DateTime.UtcNow.AddDays(1);
        private DateTime _endTime = DateTime.UtcNow.AddDays(1).AddHours(2);
        private string _hall = "Hall 1";
        private decimal _price = 10.99m;
        private decimal _ticketPrice = 10.99m;
        private int _totalSeats = 100;
        private bool _isActive = true;
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt = null;

        public ShowtimeBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ShowtimeBuilder WithMovieId(Guid movieId)
        {
            _movieId = movieId;
            return this;
        }

        public ShowtimeBuilder WithStartTime(DateTime startTime)
        {
            _startTime = startTime;
            return this;
        }

        public ShowtimeBuilder WithEndTime(DateTime endTime)
        {
            _endTime = endTime;
            return this;
        }

        public ShowtimeBuilder WithHall(string hall)
        {
            _hall = hall;
            return this;
        }

        public ShowtimeBuilder WithPrice(decimal price)
        {
            _price = price;
            _ticketPrice = price; // Keep them synced
            return this;
        }

        public ShowtimeBuilder WithTotalSeats(int totalSeats)
        {
            _totalSeats = totalSeats;
            return this;
        }

        public ShowtimeBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public ShowtimeBuilder WithCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public ShowtimeBuilder WithUpdatedAt(DateTime? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public Showtime Build()
        {
            return new Showtime
            {
                Id = _id,
                MovieId = _movieId,
                StartTime = _startTime,
                EndTime = _endTime,
                Hall = _hall,
                Price = _price,
                TicketPrice = _ticketPrice,
                TotalSeats = _totalSeats,
                IsActive = _isActive,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt,
                Seats = new List<Seat>(),
                Reservations = new List<Reservation>()
            };
        }
    }
}
