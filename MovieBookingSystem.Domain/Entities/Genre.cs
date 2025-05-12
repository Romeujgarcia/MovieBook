using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        // Relacionamentos
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}