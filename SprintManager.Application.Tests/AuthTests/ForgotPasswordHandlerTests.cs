using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.AuthTests
{
    public class ForgotPasswordHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly ForgotPasswordHandler _handler;

        public ForgotPasswordHandlerTests()
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
            _mockEmailSender = new Mock<IEmailSender>();

            // Initialize handler injecting the mocks
            _handler = new ForgotPasswordHandler(
                _mockUserManager.Object,
                _mockEmailSender.Object
            );
        }

        // Test handler - forgot password (when email exists)
        [Fact]
        public async Task Handle_ForgotPassword_WhenEmailExists()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "d@gmail.com"
            };

            var user = new User("Daniel", "daniazevedo43", command.Email, "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(It.IsAny<string>());
            _mockEmailSender.Setup(r => r.SendEmailAsync(command.Email, "Reset Password", It.IsAny<string>()));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure GeneratePasswordResetTokenAsync was called exactly once.
            _mockUserManager.Verify(r => r.GeneratePasswordResetTokenAsync(user), Times.Once);

            // Ensure SendEmailAsync was called exactly once.
            _mockEmailSender.Verify(r => r.SendEmailAsync(command.Email, "Reset Password", It.IsAny<string>()), Times.Once);
        }

        // Test handler - forgot password (when email doesn't exist)
        [Fact]
        public async Task Handle_ForgotPassword_WhenEmailDoesNotExist()
        {
            var command = new ForgotPasswordCommand
            {
                Email = "d@gmail.com"
            };

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);
        }
    }
}