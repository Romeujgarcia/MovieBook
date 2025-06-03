using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Infrastructure.Data;
using MovieBookingSystem.Api;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MovieBookingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using MovieBookingSystem.API.Authentication;
using Xunit;

namespace MovieBookingSystem.Tests.Common.Fixtures
{
    public class WebApiFixture : WebApplicationFactory<Program>, IDisposable
    {
        private readonly string _databaseName;

        public WebApiFixture()
        {
            // Use a unique database name for each test run to avoid conflicts
            _databaseName = $"InMemoryDbForTesting_{Guid.NewGuid()}";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove the app's DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MovieBookingDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add DbContext using an in-memory database for testing
                services.AddDbContext<MovieBookingDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });

                // Clear existing authentication services
                services.RemoveAll<IAuthenticationService>();
                services.RemoveAll<IAuthenticationSchemeProvider>();

                // Add test authentication
                services.AddAuthentication(defaultScheme: "Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                        "Test", options => { });

                // Build service provider and seed database
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MovieBookingDbContext>();

                db.Database.EnsureCreated();
                SeedDatabase(db);
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["JwtSettings:Secret"] = "this-is-a-very-long-secret-key-for-testing-purposes-that-meets-minimum-requirements",
                    ["JwtSettings:Issuer"] = "TestIssuer",
                    ["JwtSettings:Audience"] = "TestAudience",
                    ["JwtSettings:DurationInMinutes"] = "60"
                });
            });
        }

        public HttpClient GetAuthenticatedClient()
        {
            var client = CreateClient();
            return client;
        }

        private void SeedDatabase(MovieBookingDbContext db)
        {
            if (!db.Users.Any())
            {
                var passwordHasher = new PasswordHasher();

                var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var regularUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");

                var admin = new User
                {
                    Id = adminId,
                    UserName = "admin",
                    Email = "admin@example.com",
                    PasswordHash = passwordHasher.HashPassword("AdminPassword123!"),
                    IsAdmin = true,
                    FullName = "Admin User",
                    CreatedAt = DateTime.UtcNow
                };

                var regularUser = new User
                {
                    Id = regularUserId,
                    UserName = "user",
                    Email = "user@example.com",
                    PasswordHash = passwordHasher.HashPassword("UserPassword123!"),
                    IsAdmin = false,
                    FullName = "Regular User",
                    CreatedAt = DateTime.UtcNow
                };

                db.Users.AddRange(admin, regularUser);

                var actionGenre = new Genre { Id = Guid.NewGuid(), Name = "Action" };
                db.Genres.Add(actionGenre);

                var movieId = Guid.Parse("33333333-3333-3333-3333-333333333333");
                var movie = new Movie
                {
                    Id = movieId,
                    Title = "Test Movie",
                    Description = "Test Description",
                    PosterImage = "test.jpg",
                    DurationMinutes = 120,
                    CreatedAt = DateTime.UtcNow
                };
                db.Movies.Add(movie);

                db.MovieGenres.Add(new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = actionGenre.Id
                });

                var showtimeId = Guid.Parse("44444444-4444-4444-4444-444444444444");
                var showtime = new Showtime
                {
                    Id = showtimeId,
                    MovieId = movie.Id,
                    StartTime = DateTime.UtcNow.AddDays(1),
                    EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                    Hall = "Hall 1",
                    Price = 10.99m,
                    TicketPrice = 10.99m,
                    TotalSeats = 100,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                db.Showtimes.Add(showtime);

                // Seed seats
                for (int i = 0; i < 10; i++)
                {
                    string row = ((char)('A' + i)).ToString();
                    for (int j = 1; j <= 10; j++)
                    {
                        db.Seats.Add(new Seat
                        {
                            Id = Guid.NewGuid(),
                            ShowtimeId = showtime.Id,
                            Row = row,
                            SeatNumber = j,
                            IsAvailable = true,
                            IsReserved = false,
                            Type = SeatType.Regular
                        });
                    }
                }

                db.SaveChanges();
            }
        }

        public Guid GetAdminUserId()
        {
            return Guid.Parse("11111111-1111-1111-1111-111111111111");
        }

        public Guid GetShowtimeId()
        {
            return Guid.Parse("44444444-4444-4444-4444-444444444444");
        }

        public Guid GetAvailableSeatId()
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieBookingDbContext>();
            return dbContext.Seats
                .First(s => s.ShowtimeId == GetShowtimeId() && s.IsAvailable && !s.IsReserved).Id;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Limpar o banco de dados em memória
                using var scope = Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MovieBookingDbContext>();
                dbContext.Database.EnsureDeleted();
            }
            base.Dispose(disposing);
        }

    }

    // Collection definition for xUnit
    [CollectionDefinition("WebApi Collection")]
    public class WebApiCollectionFixture : ICollectionFixture<WebApiFixture>
    {
        // This class has no code, and is never created. Its purpose is just
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    public static class ServiceCollectionExtensions
    {
        public static void RemoveAll<T>(this IServiceCollection services)
        {
            var descriptors = services.Where(d => d.ServiceType == typeof(T)).ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }
    }

}