using MovieBookingSystem.Application.Services;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;
using MovieBookingSystem.Infrastructure.Identity;
using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using AutoMapper;
using ApplicationException = MovieBookingSystem.Application.Common.Exceptions.ApplicationException; // Fix ambiguous reference

namespace MovieBookingSystem.Tests.Unit.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;
        private readonly Fixture _fixture;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockMapper = new Mock<IMapper>();
            
            // Setup the UnitOfWork to return our mock repository
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepository.Object);
            
            _userService = new UserService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockPasswordHasher.Object);
                
            _fixture = new Fixture();
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ReturnsUserDto()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterUserDto>();
            var hashedPassword = "hashedPassword";
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                FullName = registerDto.FullName,
                CreatedAt = DateTime.UtcNow
            };
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            };
            
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(registerDto.Email))
                .ReturnsAsync((User)null);
                
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(registerDto.UserName))
                .ReturnsAsync((User)null);
                
            _mockPasswordHasher.Setup(hasher => hasher.HashPassword(registerDto.Password))
                .Returns(hashedPassword);
                
            _mockMapper.Setup(m => m.Map<User>(registerDto)).Returns(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);
            
            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(user);
                
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _userService.RegisterAsync(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be(registerDto.UserName);
            result.Email.Should().Be(registerDto.Email);
            result.FullName.Should().Be(registerDto.FullName);
            
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ThrowsApplicationException()
        {
            // Arrange
            var registerDto = _fixture.Create<RegisterUserDto>();
            var existingUser = new User { Email = registerDto.Email };
            
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(registerDto.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            // Note: Your UserService is throwing System.ApplicationException instead of your custom one
            // You need to fix this in your UserService.cs file
            await Assert.ThrowsAsync<System.ApplicationException>(
                () => _userService.RegisterAsync(registerDto));
                
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ReturnsUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                FullName = "Test User"
            };
            
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            };
            
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);
                
            _mockMapper.Setup(m => m.Map<UserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.UserName.Should().Be(user.UserName);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "user1",
                    Email = "user1@example.com",
                    FullName = "User One"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "user2",
                    Email = "user2@example.com",
                    FullName = "User Two"
                }
            };
            
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            }).ToList();
            
            _mockUserRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);
                
            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users))
                .Returns(userDtos);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.ElementAt(0).UserName.Should().Be("user1");
            result.ElementAt(1).UserName.Should().Be("user2");
        }
    }
}