using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization; // Adicione esta linha
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Infrastructure.Data;
using MovieBookingSystem.Domain.Entities;
using System.Collections.Generic;
using System.Net;

// Importando o namespace onde ApiResponse<T>, ReservationDto e SeatDto estão definidos
using MovieBookingSystem.Application.DTOs;

namespace MovieBookingSystem.Tests.Integration.API.Controllers
{
    public class ReservationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public ReservationsControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateAndRetrieveReservation_ShouldWorkWithIsolatedDb()
        {
            // Arrange - Setup test data
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieBookingDbContext>();

            try
            {
                // Ensure database is clean
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                // Create test data
                var movie = new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Movie",
                    Description = "Test Description",
                    PosterImage = "testposter.jpg",
                    DurationMinutes = 120,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var showtime = new Showtime
                {
                    Id = Guid.NewGuid(),
                    MovieId = movie.Id,
                    StartTime = DateTime.UtcNow.AddHours(2),
                    EndTime = DateTime.UtcNow.AddHours(4),
                    Hall = "Hall 1",
                    Price = 15.00m,
                    TicketPrice = 15.00m,
                    TotalSeats = 100,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Movie = movie
                };

                var seat1 = new Seat
                {
                    Id = Guid.NewGuid(),
                    ShowtimeId = showtime.Id,
                    Row = "A",
                    SeatNumber = 1,
                    IsAvailable = true,
                    IsReserved = false,
                    Type = SeatType.Regular
                };

                var seat2 = new Seat
                {
                    Id = Guid.NewGuid(),
                    ShowtimeId = showtime.Id,
                    Row = "A",
                    SeatNumber = 2,
                    IsAvailable = true,
                    IsReserved = false,
                    Type = SeatType.Regular
                };

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "testuser",
                    Email = "test@example.com",
                    PasswordHash = "hashedpassword",
                    FullName = "Test User Full Name",
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Add entities to context
                context.Movies.Add(movie);
                context.Showtimes.Add(showtime);
                context.Seats.AddRange(seat1, seat2);
                context.Users.Add(user);

                // Save changes
                await context.SaveChangesAsync();

                // Create reservation request
                var createReservationDto = new
                {
                    UserId = user.Id,
                    ShowtimeId = showtime.Id,
                    SeatIds = new List<Guid> { seat1.Id, seat2.Id }
                };

                var json = JsonSerializer.Serialize(createReservationDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Act - Create reservation
                var response = await _client.PostAsync("/api/reservations", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"Response Content: {responseContent}");
                _output.WriteLine($"Response Status Code: {response.StatusCode}");

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new JsonStringEnumConverter()); // Adicionando o conversor de enum
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ReservationDto>>(responseContent, options);

                Assert.NotNull(apiResponse);
                Assert.True(apiResponse.Success);
                Assert.NotNull(apiResponse.Data);

                var createdReservation = apiResponse.Data;
                Assert.NotEqual(Guid.Empty, createdReservation.Id);
                Assert.Equal(user.Id, createdReservation.UserId);
                Assert.Equal(showtime.Id, createdReservation.ShowtimeId);
                Assert.Equal(2, createdReservation.Seats.Count);

                // Verify reservation was saved to database
                var savedReservation = await context.Reservations
                    .Include(r => r.ReservationSeats)
                    .FirstOrDefaultAsync(r => r.Id == createdReservation.Id);

                Assert.NotNull(savedReservation);
                Assert.Equal(2, savedReservation.ReservationSeats.Count);
            }
            catch (Exception ex)
            {
                throw new Exception($"Test failed with exception: {ex.Message}", ex);
            }
        }

        [Fact]
        public async Task CancelReservation_ShouldReturnNotFound_WhenReservationDoesNotExist()
        {
            // Arrange
            var nonExistentReservationId = Guid.NewGuid();
            // Act - Attempt to cancel a non-existent reservation
            var response = await _client.DeleteAsync($"/api/reservations/{nonExistentReservationId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(apiResponse);
            Assert.False(apiResponse.Success);
            Assert.NotNull(apiResponse.Message);
        }
    }
}
