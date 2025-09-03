using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Auth;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.AuthTests
{
    public class RegisterHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
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
            _handler = new RegisterHandler(_mockUserManager.Object, _mockMapper.Object);
        }

        // Test handler - registration success
        [Fact]
        public async Task Handle_RegistersUser_ReturnsUserDTO()
        {
            var command = new RegisterCommand
            {
                Name = "Daniel",
                UserName = "daniazevedo43",
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var userDTO = new UserDTO
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                UserName = command.UserName,
                Email = command.Email
            };

            // Repositories Mock configuration
            _mockUserManager.Setup(r => r.FindByNameAsync(command.UserName));
            _mockUserManager.Setup(r => r.FindByEmailAsync(command.Email));
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<User>(), command.Password))
                .Callback<User, string>((u, p) => u.Id = Guid.NewGuid());

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDTO.Name, result.Name);
            Assert.Equal(userDTO.UserName, result.UserName);
            Assert.Equal(userDTO.Email, result.Email);

            // Ensure FindByNameAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByNameAsync(command.UserName), Times.Once);

            // Ensure FindByEmailAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByEmailAsync(command.Email), Times.Once);

            // Ensure CreateAsync was called exactly once.
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<UserDTO>(It.IsAny<User>()), Times.Once);
        }
    }
}