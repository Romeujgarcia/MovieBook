using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Infrastructure.Data;

namespace MovieBookingSystem.Tests.Common.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public readonly MovieBookingDbContext DbContext;
        public readonly IServiceProvider ServiceProvider;

        public DatabaseFixture()
        {
            var services = new ServiceCollection();

            services.AddDbContext<MovieBookingDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "TestMovieBookingDb"));

            // Add other services as needed for your tests
            
            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<MovieBookingDbContext>();

            // Ensure the database is created
            DbContext.Database.EnsureCreated();

            // Seed the database with test data
            SeedDatabase();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        private void SeedDatabase()
        {
            // Add test data similar to what was in WebApiFixture
            // This is sample code - adapt to your specific needs
            if (!DbContext.Users.Any())
            {
                // Seed users
                var admin = new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    UserName = "admin",
                    Email = "admin@example.com",
                    PasswordHash = "hashed_admin_password",
                    IsAdmin = true,
                    FullName = "Admin User",
                    CreatedAt = DateTime.UtcNow
                };

                var user = new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    UserName = "user",
                    Email = "user@example.com",
                    PasswordHash = "hashed_user_password",
                    IsAdmin = false,
                    FullName = "Regular User",
                    CreatedAt = DateTime.UtcNow
                };

                DbContext.Users.AddRange(admin, user);

                // Seed genres
                var actionGenre = new Genre 
                { 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), 
                    Name = "Action" 
                };
                
                var comedyGenre = new Genre 
                { 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), 
                    Name = "Comedy" 
                };

                DbContext.Genres.AddRange(actionGenre, comedyGenre);

                // Seed movies
                var movie1 = new Movie
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                    Title = "Test Action Movie",
                    Description = "An exciting action movie for testing",
                    DurationMinutes = 120,
                    PosterImage = "action.jpg",
                    CreatedAt = DateTime.UtcNow
                };

                var movie2 = new Movie
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
                    Title = "Test Comedy Movie",
                    Description = "A funny comedy movie for testing",
                    DurationMinutes = 95,
                    PosterImage = "comedy.jpg",
                    CreatedAt = DateTime.UtcNow
                };

                DbContext.Movies.AddRange(movie1, movie2);

                // Add movie genres
                DbContext.MovieGenres.AddRange(
                    new MovieGenre
                    {
                        MovieId = movie1.Id,
                        GenreId = actionGenre.Id
                    },
                    new MovieGenre
                    {
                        MovieId = movie2.Id,
                        GenreId = comedyGenre.Id
                    }
                );

                // Seed showtimes
                var showtime1 = new Showtime
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000030"),
                    MovieId = movie1.Id,
                    StartTime = DateTime.UtcNow.AddDays(1),
                    EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                    Hall = "Hall 1",
                    Price = 10.99m,
                    TicketPrice = 10.99m,
                    TotalSeats = 20,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var showtime2 = new Showtime
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000031"),
                    MovieId = movie2.Id,
                    StartTime = DateTime.UtcNow.AddDays(2),
                    EndTime = DateTime.UtcNow.AddDays(2).AddHours(1.5),
                    Hall = "Hall 2",
                    Price = 8.99m,
                    TicketPrice = 8.99m,
                    TotalSeats = 20,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                DbContext.Showtimes.AddRange(showtime1, showtime2);

                // Seed seats
                for (int i = 0; i < 2; i++) // A-B
                {
                    string row = ((char)('A' + i)).ToString();
                    for (int j = 1; j <= 10; j++) // 1-10
                    {
                        DbContext.Seats.Add(new Seat
                        {
                            Id = Guid.NewGuid(),
                            ShowtimeId = showtime1.Id,
                            Row = row,
                            SeatNumber = j,
                            IsAvailable = true,
                            IsReserved = false,
                            Type = i == 0 ? SeatType.Regular : SeatType.Premium
                        });

                        DbContext.Seats.Add(new Seat
                        {
                            Id = Guid.NewGuid(),
                            ShowtimeId = showtime2.Id,
                            Row = row,
                            SeatNumber = j,
                            IsAvailable = true,
                            IsReserved = false,
                            Type = i == 0 ? SeatType.Regular : SeatType.Premium
                        });
                    }
                }

                DbContext.SaveChanges();
            }
        }
    }
}