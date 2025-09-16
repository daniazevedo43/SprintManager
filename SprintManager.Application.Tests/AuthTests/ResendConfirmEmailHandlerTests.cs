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
    public class ResendConfirmEmailHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly ResendConfirmationEmailHandler _handler;

        public ResendConfirmEmailHandlerTests()
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
            _handler = new ResendConfirmationEmailHandler(
                _mockUserManager.Object,
                _mockEmailSender.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_ResendConfirmationEmail()
        {
            var command = new ResendConfirmationEmailCommand
            {
                Email = "d@gmail.com"
            };

            var user = new User("Daniel", "daniazevedo43", command.Email, "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.GenerateEmailConfirmationTokenAsync(user));
            _mockEmailSender.Setup(r => r.SendEmailAsync(command.Email, "Confirm your email", It.IsAny<string>()));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure GenerateEmailConfirmationTokenAsync was called exactly once.
            _mockUserManager.Verify(r => r.GenerateEmailConfirmationTokenAsync(user), Times.Once);

            // Ensure SendEmailAsync was called exactly once.
            _mockEmailSender.Verify(r => r.SendEmailAsync(command.Email, "Confirm your email", It.IsAny<string>()), Times.Once);
        }
    }
}