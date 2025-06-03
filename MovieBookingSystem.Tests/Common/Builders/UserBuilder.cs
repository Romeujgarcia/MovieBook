using System;
using MovieBookingSystem.Domain.Entities;

namespace MovieBookingSystem.Tests.Common.Builders
{
    public class UserBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _userName = "testuser";
        private string _email = "test@example.com";
        private string _passwordHash = "hashed_password";
        private string _fullName = "Test User";
        private bool _isAdmin = false;
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt = null;

        public UserBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public UserBuilder WithUserName(string userName)
        {
            _userName = userName;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder WithPasswordHash(string passwordHash)
        {
            _passwordHash = passwordHash;
            return this;
        }

        public UserBuilder WithFullName(string fullName)
        {
            _fullName = fullName;
            return this;
        }

        public UserBuilder WithIsAdmin(bool isAdmin)
        {
            _isAdmin = isAdmin;
            return this;
        }

        public UserBuilder WithCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public UserBuilder WithUpdatedAt(DateTime? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Id = _id,
                UserName = _userName,
                Email = _email,
                PasswordHash = _passwordHash,
                FullName = _fullName,
                IsAdmin = _isAdmin,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt,
                Reservations = new List<Reservation>()
            };
        }
    }
}