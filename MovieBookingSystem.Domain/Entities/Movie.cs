using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImage { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Relacionamentos
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
    }
}
