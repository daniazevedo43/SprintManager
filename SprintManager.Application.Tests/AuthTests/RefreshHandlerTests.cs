using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Tests.AuthTests
{
    public class RefreshHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly RefreshHandler _handler;

        public RefreshHandlerTests()
        {
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockTokenService = new Mock<ITokenService>();

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
            _handler = new RefreshHandler(
                _mockRefreshTokenRepository.Object,
                _mockTokenService.Object,
                _mockUserManager.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_Refresh_ReturnsNewAccessAndRefreshToken()
        {
            var command = new RefreshCommand
            {
                RefreshToken = "test_token"
            };

            var user = new User("Test", "test", "test@gmail.com", "Test123test123!");

            var refreshToken = new RefreshToken(user.Id, "test_token");

            var securityToken = new JwtSecurityToken();
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var newRefreshToken = new RefreshToken(user.Id, "new_token");

            _mockRefreshTokenRepository.Setup(r => r.GetByTokenAsync(command.RefreshToken)).ReturnsAsync(refreshToken);
            _mockUserManager.Setup(r => r.FindByIdAsync(refreshToken.UserId.ToString())).ReturnsAsync(user);
            _mockRefreshTokenRepository.Setup(r => r.DeleteAsync(refreshToken));
            _mockTokenService.Setup(r => r.CreateToken(user)).Returns(securityToken);
            _mockTokenService.Setup(r => r.GenerateRefreshToken()).Returns(newRefreshToken.Token);
            _mockRefreshTokenRepository.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Callback<RefreshToken>(t => newRefreshToken = t);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(jwtToken, result.AccessToken);
            Assert.Equal(newRefreshToken.Token, result.RefreshToken);

            // Ensure GetByTokenAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.GetByTokenAsync(command.RefreshToken), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(refreshToken.UserId.ToString()), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockRefreshTokenRepository.Setup(r => r.DeleteAsync(refreshToken));

            // Ensure CreateToken was called exactly once.
            _mockTokenService.Verify(r => r.CreateToken(user), Times.Once);

            // Ensure GenerateRefreshToken was called exactly once.
            _mockTokenService.Verify(r => r.GenerateRefreshToken(), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
        }

        [Fact]
        public async Task VerifyRefreshToken_ThrowsException_WhenInvalidOrExpiredRefreshToken()
        {
            var command = new RefreshCommand
            {
                RefreshToken = "test_token"
            };

            _mockRefreshTokenRepository.Setup(r => r.GetByTokenAsync(command.RefreshToken));

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Invalid or expired refresh token.", exception.Message);

            // Ensure GetByTokenAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.GetByTokenAsync(command.RefreshToken), Times.Once);
        }

        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserWasNotFound()
        {
            var command = new RefreshCommand
            {
                RefreshToken = "test_token"
            };

            var user = new User("Test", "test", "test@gmail.com", "Test123test123!");

            var refreshToken = new RefreshToken(user.Id, "test_token");

            _mockRefreshTokenRepository.Setup(r => r.GetByTokenAsync(command.RefreshToken)).ReturnsAsync(refreshToken);
            _mockUserManager.Setup(r => r.FindByIdAsync(refreshToken.UserId.ToString()));

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not found.", exception.Message);

            // Ensure GetByTokenAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.GetByTokenAsync(command.RefreshToken), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(refreshToken.UserId.ToString()), Times.Once);
        }
    }
}