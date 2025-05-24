using System;
using System.Collections.Generic;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Tests.Common.Builders
{
    public class MovieBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _title = "Test Movie";
        private string _description = "Test Description";
        private string _posterImage = "test.jpg";
        private int _durationMinutes = 120;
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt = null;
        private List<Guid> _genreIds = new List<Guid>();

        public MovieBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public MovieBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public MovieBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public MovieBuilder WithPosterImage(string posterImage)
        {
            _posterImage = posterImage;
            return this;
        }

        public MovieBuilder WithDurationMinutes(int durationMinutes)
        {
            _durationMinutes = durationMinutes;
            return this;
        }

        public MovieBuilder WithCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public MovieBuilder WithUpdatedAt(DateTime? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public MovieBuilder WithGenre(Guid genreId)
        {
            _genreIds.Add(genreId);
            return this;
        }

        public Movie Build()
        {
            var movie = new Movie
            {
                Id = _id,
                Title = _title,
                Description = _description,
                PosterImage = _posterImage,
                DurationMinutes = _durationMinutes,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt,
                MovieGenres = new List<MovieGenre>(),
                Showtimes = new List<Showtime>()
            };

            foreach (var genreId in _genreIds)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    MovieId = _id,
                    GenreId = genreId
                });
            }

            return movie;
        }
    }
}