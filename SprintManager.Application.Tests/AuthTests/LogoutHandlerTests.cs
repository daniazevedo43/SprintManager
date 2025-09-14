using Microsoft.AspNetCore.Http;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Interfaces;
using System.Security.Claims;

namespace SprintManager.Application.Tests.AuthTests
{
    public class LogoutHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly LogoutHandler _handler;

        public LogoutHandlerTests()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _handler = new LogoutHandler(
                _mockHttpContextAccessor.Object,
                _mockRefreshTokenRepository.Object
            );
        }

        // Test handler - logout success
        [Fact]
        public async Task Handle_UserLogout_DeletesAllRefreshTokens()
        {
            var command = new LogoutCommand();

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockRefreshTokenRepository
                .Setup(r => r.DeleteAllByUserIdAsync(userId));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure DeleteAllByUserIdAsync was called exactly once.
            _mockRefreshTokenRepository
                .Verify(r => r.DeleteAllByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task VerifyUserId_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new LogoutCommand();

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
    }
}