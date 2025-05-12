using System;

namespace MovieBookingSystem.Domain.Entities
{
    // Tabela de junção entre Movie e Genre (muitos para muitos)
    public class MovieGenre
    {
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
