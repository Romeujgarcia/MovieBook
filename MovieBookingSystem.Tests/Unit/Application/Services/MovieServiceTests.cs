using MovieBookingSystem.Application.Services;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using AutoMapper;
using NotFoundException = MovieBookingSystem.Application.Common.Exceptions.NotFoundException;

namespace MovieBookingSystem.Tests.Unit.Application.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMovieRepository> _mockMovieRepo;
        private readonly Mock<IGenreRepository> _mockGenreRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MovieService _movieService;
        private readonly Fixture _fixture;

        public MovieServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMovieRepo = new Mock<IMovieRepository>();
            _mockGenreRepo = new Mock<IGenreRepository>();
            _mockMapper = new Mock<IMapper>();
            
            // Setup UnitOfWork to return our mock repositories
            _mockUnitOfWork.Setup(uow => uow.Movies).Returns(_mockMovieRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Genres).Returns(_mockGenreRepo.Object);
            
            // MovieService likely takes IUnitOfWork and IMapper
            _movieService = new MovieService(_mockUnitOfWork.Object, _mockMapper.Object);
                
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ReturnsMovieDto()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var movie = new Movie
            {
                Id = movieId,
                Title = "Test Movie",
                Description = "Test Description",
                PosterImage = "test.jpg", // This property exists in Movie entity
                DurationMinutes = 120,
                CreatedAt = DateTime.UtcNow.Date,
                UpdatedAt = DateTime.UtcNow.Date
            };
            
            var movieDto = new MovieDto
            {
                Id = movieId,
                Title = "Test Movie",
                Description = "Test Description",
                PosterUrl = "test.jpg", // MovieDto uses PosterUrl, not PosterImage
                DurationMinutes = 120
                // MovieDto doesn't have CreatedAt/UpdatedAt properties
            };
            
            _mockMovieRepo.Setup(repo => repo.GetByIdAsync(movieId))
                .ReturnsAsync(movie);
                
            _mockMapper.Setup(m => m.Map<MovieDto>(movie))
                .Returns(movieDto);

            // Act
            var result = await _movieService.GetByIdAsync(movieId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(movieId);
            result.Title.Should().Be("Test Movie");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ThrowsNotFoundException()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            
            _mockMovieRepo.Setup(repo => repo.GetByIdAsync(movieId))
                .ReturnsAsync((Movie)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _movieService.GetByIdAsync(movieId));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = Guid.NewGuid(), Title = "Movie 1" },
                new Movie { Id = Guid.NewGuid(), Title = "Movie 2" }
            };
            
            var movieDtos = movies.Select(m => new MovieDto 
            { 
                Id = m.Id, 
                Title = m.Title 
            }).ToList();
            
            _mockMovieRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(movies);
                
            _mockMapper.Setup(m => m.Map<IEnumerable<MovieDto>>(movies))
                .Returns(movieDtos);

            // Act
            var result = await _movieService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddAsync_WithValidData_ReturnsMovieDto()
        {
            // Arrange
            var createDto = new CreateMovieDto
            {
                Title = "New Movie",
                Description = "New Description",
                DurationMinutes = 150, // Use DurationMinutes, not Duration
                PosterUrl = "poster.jpg", // Add PosterUrl if needed
                GenreIds = new List<Guid> { Guid.NewGuid() }
            };
            
            var genre = new Genre { Id = createDto.GenreIds.First(), Name = "Action" };
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                DurationMinutes = createDto.DurationMinutes, // Use DurationMinutes
                PosterImage = createDto.PosterUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes, // Use DurationMinutes
                PosterUrl = movie.PosterImage
            };
            
            _mockGenreRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(genre);
                
            _mockMapper.Setup(m => m.Map<Movie>(createDto))
                .Returns(movie);
                
            _mockMapper.Setup(m => m.Map<MovieDto>(movie))
                .Returns(movieDto);
                
            _mockMovieRepo.Setup(repo => repo.AddAsync(It.IsAny<Movie>()))
                .ReturnsAsync(movie);
                
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _movieService.AddAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(createDto.Title);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WithNonExistingGenre_ThrowsNotFoundException()
        {
            // Arrange
            var createDto = new CreateMovieDto
            {
                Title = "New Movie",
                GenreIds = new List<Guid> { Guid.NewGuid() }
            };
            
            _mockGenreRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Genre)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _movieService.AddAsync(createDto));
        }
    }
}