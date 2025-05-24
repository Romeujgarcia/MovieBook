using MovieBookingSystem.Application.Services;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using AutoMapper;
using MovieBookingSystem.Infrastructure.Data;
using ApplicationException = MovieBookingSystem.Application.Common.Exceptions.ApplicationException;

namespace MovieBookingSystem.Tests.Unit.Application.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReservationRepository> _mockReservationRepo;
        private readonly Mock<IShowtimeRepository> _mockShowtimeRepo;
        private readonly Mock<ISeatRepository> _mockSeatRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ReservationService _reservationService;
        private readonly Fixture _fixture;

        public ReservationServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockReservationRepo = new Mock<IReservationRepository>();
            _mockShowtimeRepo = new Mock<IShowtimeRepository>();
            _mockSeatRepo = new Mock<ISeatRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Setup UnitOfWork to return our mock repositories
            _mockUnitOfWork.Setup(uow => uow.Reservations).Returns(_mockReservationRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Showtimes).Returns(_mockShowtimeRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Seats).Returns(_mockSeatRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepo.Object);

            _reservationService = new ReservationService(_mockUnitOfWork.Object, _mockMapper.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateReservationAsync_WithValidData_ReturnsReservationDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var showtimeId = Guid.NewGuid();
            var movieId = Guid.NewGuid();
            var seatIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var createDto = new CreateReservationDto
            {
                ShowtimeId = showtimeId,
                SeatIds = seatIds
            };

            // Setup User
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            // Setup Movie
            var movie = new Movie
            {
                Id = movieId,
                Title = "Test Movie",
                Description = "Test Description",
                PosterImage = "http://example.com/poster.jpg",
                DurationMinutes = 120,
                CreatedAt = DateTime.UtcNow
            };

            // Setup Showtime
            var showtime = new Showtime
            {
                Id = showtimeId,
                MovieId = movieId,
                Movie = movie,
                StartTime = DateTime.UtcNow.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                Hall = "Hall 1",
                Price = 10.0m,
                TicketPrice = 10.0m,
                TotalSeats = 100,
                IsActive = true
            };

            // Setup Seats
            var seats = seatIds.Select((id, index) => new Seat
            {
                Id = id,
                ShowtimeId = showtimeId,
                Row = "A",
                SeatNumber = index + 1,
                Type = SeatType.Regular,
                IsAvailable = true,
                IsReserved = false
            }).ToList();

            // Setup expected reservation
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                ShowtimeId = showtimeId,
                UserId = userId,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = 20.0m,
                ReservationSeats = seatIds.Select(seatId => new ReservationSeat
                {
                    SeatId = seatId
                }).ToList()
            };

            var reservationDto = new ReservationDto
            {
                Id = reservation.Id,
                ShowtimeId = showtimeId,
                UserId = userId,
                Status = ReservationStatus.Confirmed,
                UserName = user.UserName,
                MovieTitle = movie.Title,
                ShowtimeStart = showtime.StartTime,
                TotalPrice = 20.0m,
                Seats = seats.Select(s => new SeatDto 
                { 
                    Id = s.Id,
                    ShowtimeId = s.ShowtimeId,
                    Row = s.Row,
                    SeatNumber = s.SeatNumber,
                    IsReserved = false
                }).ToList()
            };

            // Setup mocks
            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockShowtimeRepo.Setup(repo => repo.GetByIdAsync(showtimeId))
                .ReturnsAsync(showtime);

            _mockSeatRepo.Setup(repo => repo.GetByIdsAsync(seatIds))
                .ReturnsAsync(seats);

            _mockSeatRepo.Setup(repo => repo.IsSeatAvailableAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _mockReservationRepo.Setup(repo => repo.AddAsync(It.IsAny<Reservation>()))
                .ReturnsAsync(reservation);

            

            _mockMapper.Setup(m => m.Map<ReservationDto>(It.IsAny<Reservation>()))
                .Returns(reservationDto);

            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _reservationService.CreateReservationAsync(createDto, userId);

            // Assert
            result.Should().NotBeNull();
            result.ShowtimeId.Should().Be(showtimeId);
            result.UserId.Should().Be(userId);
            result.Status.Should().Be(ReservationStatus.Confirmed);
            result.Seats.Should().HaveCount(seatIds.Count);
            result.UserName.Should().Be(user.UserName);
            result.MovieTitle.Should().Be(movie.Title);

            // Verify method calls
            _mockUserRepo.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockShowtimeRepo.Verify(repo => repo.GetByIdAsync(showtimeId), Times.Once);
            _mockSeatRepo.Verify(repo => repo.GetByIdsAsync(seatIds), Times.Once);
            _mockSeatRepo.Verify(repo => repo.IsSeatAvailableAsync(It.IsAny<Guid>()), Times.Exactly(seatIds.Count));
            _mockReservationRepo.Verify(repo => repo.AddAsync(It.IsAny<Reservation>()), Times.Once);
            _mockSeatRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Seat>()), Times.Exactly(seats.Count));
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateReservationAsync_WithUnavailableSeats_ThrowsApplicationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var showtimeId = Guid.NewGuid();
            var movieId = Guid.NewGuid();
            var seatIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var createDto = new CreateReservationDto
            {
                ShowtimeId = showtimeId,
                SeatIds = seatIds
            };

            // Setup User
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            // Setup Movie
            var movie = new Movie
            {
                Id = movieId,
                Title = "Test Movie",
                DurationMinutes = 120,
                Description = "Test Description",
                PosterImage = "http://example.com/poster.jpg"
            };

            // Setup Showtime
            var showtime = new Showtime
            {
                Id = showtimeId,
                MovieId = movieId,
                Movie = movie,
                StartTime = DateTime.UtcNow.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                Hall = "Hall 1",
                Price = 10.0m,
                TicketPrice = 10.0m,
                TotalSeats = 100,
                IsActive = true
            };

            // Setup Seats (but they will be unavailable)
            var seats = seatIds.Select((id, index) => new Seat
            {
                Id = id,
                ShowtimeId = showtimeId,
                Row = "A",
                SeatNumber = index + 1,
                Type = SeatType.Regular,
                IsAvailable = false, // Seats are not available
                IsReserved = true
            }).ToList();

            // Setup mocks
            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockShowtimeRepo.Setup(repo => repo.GetByIdAsync(showtimeId))
                .ReturnsAsync(showtime);

            _mockSeatRepo.Setup(repo => repo.GetByIdsAsync(seatIds))
                .ReturnsAsync(seats);

            // Setup to return false for seat availability (seats are unavailable)
            _mockSeatRepo.Setup(repo => repo.IsSeatAvailableAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(
                () => _reservationService.CreateReservationAsync(createDto, userId));

            // Verify that AddAsync was never called since seats were unavailable
            _mockReservationRepo.Verify(repo => repo.AddAsync(It.IsAny<Reservation>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateReservationAsync_WithNonExistentUser_ThrowsNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var showtimeId = Guid.NewGuid();
            var seatIds = new List<Guid> { Guid.NewGuid() };

            var createDto = new CreateReservationDto
            {
                ShowtimeId = showtimeId,
                SeatIds = seatIds
            };

            // Setup mock to return null user
            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<MovieBookingSystem.Application.Common.Exceptions.NotFoundException>(
                () => _reservationService.CreateReservationAsync(createDto, userId));
        }

        [Fact]
        public async Task CreateReservationAsync_WithNonExistentShowtime_ThrowsNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var showtimeId = Guid.NewGuid();
            var seatIds = new List<Guid> { Guid.NewGuid() };

            var createDto = new CreateReservationDto
            {
                ShowtimeId = showtimeId,
                SeatIds = seatIds
            };

            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            // Setup mocks
            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockShowtimeRepo.Setup(repo => repo.GetByIdAsync(showtimeId))
                .ReturnsAsync((Showtime)null);

            // Act & Assert
            await Assert.ThrowsAsync<MovieBookingSystem.Application.Common.Exceptions.NotFoundException>(
                () => _reservationService.CreateReservationAsync(createDto, userId));
        }
    }
}