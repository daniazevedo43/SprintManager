using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Queries.Users;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.UserTests
{
    public class GetUserByIdHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserByIdHandler _handler;

        public GetUserByIdHandlerTests()
        {
            // Create mocks for UserManager constructor's dependencies
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockOptions = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var mockUserValidator = new List<IUserValidator<User>>
            {
                new Mock<IUserValidator<User>>().Object
            };
            var mockPasswordValidator = new List<IPasswordValidator<User>>
            {
                new Mock<IPasswordValidator<User>>().Object
            };
            var mockLookupNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<User>>>();

            // Initialize mock for each test
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                mockOptions.Object,
                mockPasswordHasher.Object,
                mockUserValidator,
                mockPasswordValidator,
                mockLookupNormalizer.Object,
                mockErrors.Object,
                mockServiceProvider.Object,
                mockLogger.Object
            );
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetUserByIdHandler(
                _mockUserManager.Object, 
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsUserDto()
        {
            var query = new GetUserByIdQuery
            { 
                Id = Guid.NewGuid(),
            };

            var user = new User("Test", "test", "test@gmail.com", "Test123test123!");
            var userDto = new UserDto
            { 
                Id = query.Id, 
                Name = user.Name, 
                UserName = user.UserName,
                Email = user.Email 
            };

            _mockUserManager.Setup(r => r.FindByIdAsync(query.Id.ToString())).ReturnsAsync(user);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(userDto.Id, result.Id);
            Assert.Equal(userDto.Name, result.Name);
            Assert.Equal(userDto.UserName, result.UserName);
            Assert.Equal(userDto.Email, result.Email);

            // Ensure FindByIdAsync was called exactly once with the correct ID.
            _mockUserManager.Verify(m => m.FindByIdAsync(query.Id.ToString()), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created user.
            _mockMapper.Verify(m => m.Map<UserDto>(user), Times.Once);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var query = new GetUserByIdQuery
            {
                Id = Guid.NewGuid(),
            };

            _mockUserManager.Setup(m => m.FindByIdAsync(query.Id.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"User with ID {query.Id} not found", exception.Message);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(query.Id.ToString()), Times.Once);
        }
    }
}