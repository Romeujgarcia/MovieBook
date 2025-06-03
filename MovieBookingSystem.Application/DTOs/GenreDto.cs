using System;

namespace MovieBookingSystem.Application.DTOs
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateGenreDto
    {
        public string Name { get; set; }
    }

    public class UpdateGenreDto
    {
        public string Name { get; set; }
    }
}
