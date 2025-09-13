using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.IdentityModel.Tokens.Jwt;

namespace SprintManager.Application.Tests.AuthTests
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly LoginHandler _handler;

        public LoginHandlerTests() 
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
            _mockTokenService = new Mock<ITokenService>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();

            // Initialize handler injecting the mocks
            _handler = new LoginHandler(
                _mockUserManager.Object, 
                _mockTokenService.Object,
                _mockRefreshTokenRepository.Object
            );
        }

        // Test handler - login success
        [Fact]
        public async Task Handle_UserLogin_ReturnsToken()
        {
            var command = new LoginCommand
            {
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            var securityToken = new JwtSecurityToken();
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var refreshToken = new RefreshToken(user.Id, "token");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
            _mockTokenService.Setup(r => r.CreateToken(user)).Returns(securityToken);
            _mockRefreshTokenRepository.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Callback<RefreshToken>(t => refreshToken = t);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(jwtToken, result.AccessToken);
            Assert.Equal(refreshToken.Token, result.RefreshToken);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure IsEmailConfirmedAsync was called exactly once.
            _mockUserManager.Verify(r => r.IsEmailConfirmedAsync(user), Times.Once);

            // Ensure CheckPasswordAsync was called exactly once.
            _mockUserManager.Verify(r => r.CheckPasswordAsync(user, command.Password), Times.Once);

            // Ensure CreateToken was called exactly once.
            _mockTokenService.Verify(r => r.CreateToken(user), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockRefreshTokenRepository.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
        }

        // Test exception throwing when an email or password is invalid
        [Fact]
        public async Task VerifyEmail_ThrowsException_WhenEmailOrPasswordIsInvalid()
        {
            var command = new LoginCommand
            {
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Invalid email or password.", exception.Message);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);
        }

        // Test exception throwing when an email was not confirmed
        [Fact]
        public async Task VerifyEmail_ThrowsException_WhenEmailWasNotConfirmed()
        {
            var command = new LoginCommand
            {
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.IsEmailConfirmedAsync(user)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<SprintManagerEmailNotConfirmed>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Email not confirmed.", exception.Message);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure IsEmailConfirmedAsync was called exactly once.
            _mockUserManager.Verify(r => r.IsEmailConfirmedAsync(user), Times.Once);
        }

        // Test exception throwing when a password is invalid
        [Fact]
        public async Task VerifyPassword_ThrowsException_WhenPasswordIsInvalid()
        {
            var command = new LoginCommand
            {
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User("Daniel", "daniazevedo43", "d@gmail.com", "Abc123abc123!");

            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(r => r.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Invalid password.", exception.Message);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure IsEmailConfirmedAsync was called exactly once.
            _mockUserManager.Verify(r => r.IsEmailConfirmedAsync(user), Times.Once);

            // Ensure CheckPasswordAsync was called exactly once.
            _mockUserManager.Verify(r => r.CheckPasswordAsync(user, command.Password), Times.Once);
        }
    }
}