using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.UserTests
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            // Initialize mocks for each test
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateUserHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_CreatesUser_ReturnsUserDTO()
        {
            var command = new CreateUserCommand
            {
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User(command.Name, command.Email, command.Password);
            var userDTO = new UserDTO { Id = user.Id, UserName = user.UserName, Email = user.Email };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>())).Callback<User>(u => user = u);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns(userDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.UserName, result.UserName);
            Assert.Equal(userDTO.Email, result.Email);

            // Ensure GetByEmailAsync was called exactly once.
            _mockUserRepository.Verify(r => r.GetByEmailAsync(command.Email), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockUserRepository.Verify(r => r.AddAsync(user), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created user.
            _mockMapper.Verify(m => m.Map<UserDTO>(user), Times.Once);
        }

        // Test exception throwing when an email already exists
        [Fact]
        public async Task VerifyUserEmail_ThrowsException_WhenUserEmailAlreadyExists()
        {
            var command = new CreateUserCommand
            {
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "Abc123abc123!"
            };

            var user = new User(command.Name, command.Email, command.Password);

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A user with email '{command.Email}' already exists.", exception.Message);

            // Ensure GetByEmailAsync was called exactly once.
            _mockUserRepository.Verify(r => r.GetByEmailAsync(command.Email), Times.Once);
        }
    }
}