using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.AuthTests
{
    public class ResetPasswordHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly ResetPasswordHandler _handler;

        public ResetPasswordHandlerTests()
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

            // Initialize handler injecting the mocks
            _handler = new ResetPasswordHandler(_mockUserManager.Object);
        }

        // Test handler - reset password with success
        [Fact]
        public async Task Handle_ResetPassword()
        {
            var command = new ResetPasswordCommand
            {
                Email = "d@gmail.com",
                Token = "token",
                NewPassword = "Def456def456!"
            };

            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.ResetPasswordAsync(user, command.Token, command.NewPassword)).ReturnsAsync(IdentityResult.Success);

            await _handler.Handle(command, CancellationToken.None);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure ResetPasswordAsync was called exactly once.
            _mockUserManager.Verify(r => r.ResetPasswordAsync(user, command.Token, command.NewPassword), Times.Once);
        }

        // Test exception throwing when a user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new ResetPasswordCommand
            {
                Email = "d@gmail.com",
                Token = "token",
                NewPassword = "Def456def456!"
            };

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not found.", exception.Message);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email.ToString()), Times.Once);
        }
    }
}