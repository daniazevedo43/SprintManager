using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Queries.Auth;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.AuthTests
{
    public class ConfirmEmailHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ConfirmEmailHandler _handler;

        public ConfirmEmailHandlerTests()
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

            _handler = new ConfirmEmailHandler(_mockUserManager.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ConfirmsUserEmail()
        {
            var query = new ConfirmEmailQuery
            {
                UserId = Guid.NewGuid(),
                Token = "testToken"
            };

            var user = new User("Test", "test", "test@gmail.com", "Test123test123!");

            _mockUserManager.Setup(r => r.FindByIdAsync(query.UserId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.ConfirmEmailAsync(user, query.Token)).ReturnsAsync(IdentityResult.Success);

            await _handler.Handle(query, CancellationToken.None);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(query.UserId.ToString()), Times.Once);

            // Ensure ConfirmEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.ConfirmEmailAsync(user, query.Token), Times.Once);
        }

        // Test exception throwing when a user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var query = new ConfirmEmailQuery
            {
                UserId = Guid.NewGuid(),
                Token = "testToken"
            };

            _mockUserManager.Setup(r => r.FindByIdAsync(query.UserId.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"User not found.", exception.Message);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(query.UserId.ToString()), Times.Once);
        }
    }
}