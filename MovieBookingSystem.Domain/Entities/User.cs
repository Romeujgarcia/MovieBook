using System;
using System.Collections.Generic;

namespace MovieBookingSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Relacionamentos
        public ICollection<Reservation> Reservations { get; set; }
    }
}