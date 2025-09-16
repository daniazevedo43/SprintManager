using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Tests.AuthTests
{
    public class DeleteAccountHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly DeleteAccountHandler _handler;

        public DeleteAccountHandlerTests()
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

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();

            // Initialize handler injecting the mocks
            _handler = new DeleteAccountHandler(
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object,
                _mockRefreshTokenRepository.Object,
                _mockWorkItemRepository.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidPassword_RemovesUserAccount()
        {
            var command = new DeleteAccountCommand
            {
                Password = "Test123test123!",
            };

            var user = new User("Test", "test", "test@gmail.com", command.Password);

            var workItems = new List<WorkItem>()
            {
                new WorkItem(
                    Guid.NewGuid(),
                    "Test title", WorkItemType.Task,
                    Guid.NewGuid(), user.Id,
                    "Test description",
                    WorkItemPriorityLevel.Low,
                    DateTime.UtcNow.ToUniversalTime().AddDays(1), 8
                ),
                new WorkItem(
                    Guid.NewGuid(), "Test title 2",
                    WorkItemType.Bug, Guid.NewGuid(), user.Id,
                    "Test description 2",
                    WorkItemPriorityLevel.Low,
                    DateTime.UtcNow.ToUniversalTime().AddDays(1), 8
                )
            };

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            _mockUserManager.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _mockWorkItemRepository.Setup(r => r.GetAllByUserIdAsync(user.Id)).ReturnsAsync(workItems);
            _mockWorkItemRepository.Setup(r => r.UpdateAsync(It.IsAny<WorkItem>()));
            _mockRefreshTokenRepository.Setup(r => r.DeleteAllByUserIdAsync(user.Id));
            _mockUserManager.Setup(m => m.DeleteAsync(user));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(user.Id.ToString()), Times.Once);

            // Ensure CheckPasswordAsync was called exactly once.
            _mockUserManager.Verify(m => m.CheckPasswordAsync(user, command.Password), Times.Once);

            // Ensure GetAllByUserIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetAllByUserIdAsync(user.Id), Times.Once);

            // Ensure UpdateAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.UpdateAsync(It.IsAny<WorkItem>()), Times.Exactly(workItems.Count));

            // Ensure DeleteAllByUserIdAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.DeleteAllByUserIdAsync(user.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockUserManager.Verify(r => r.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new DeleteAccountCommand
            {
                Password = "Test123test123!",
            };

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier));

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not authenticated.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);
        }

        [Fact]
        public async Task VerifyUser_ThrowException_WhenUserWasNotFound()
        {
            var command = new DeleteAccountCommand
            {
                Password = "Test123test123!",
            };

            var user = new User("Test", "test", "test@gmail.com", command.Password);

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            _mockUserManager.Setup(m => m.FindByIdAsync(user.Id.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(user.Id.ToString()), Times.Once);
        }

        [Fact]
        public async Task VerifyPassword_ThrowException_WhenPasswordIsInvalid()
        {
            var command = new DeleteAccountCommand
            {
                Password = "Test123test123!",
            };

            var user = new User("Test", "test", "test@gmail.com", command.Password);

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            _mockUserManager.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Invalid password.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(user.Id.ToString()), Times.Once);

            // Ensure CheckPasswordAsync was called exactly once.
            _mockUserManager.Verify(m => m.CheckPasswordAsync(user, command.Password), Times.Once);
        }
    }
}