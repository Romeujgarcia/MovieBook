using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Application.DTOs
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public int DurationMinutes { get; set; }
        public List<GenreDto> Genres { get; set; }
    }

    public class CreateMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public int DurationMinutes { get; set; }
        public List<Guid> GenreIds { get; set; } = new List<Guid>(); // Initialize to an empty list
    }

    public class UpdateMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public int DurationMinutes { get; set; }
        public List<Guid> GenreIds { get; set; }
    }
}