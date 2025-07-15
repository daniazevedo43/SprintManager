using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.UserTests
{
    public class UpdateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateUserHandler _handler;

        public UpdateUserHandlerTests()
        {
            // Initialize mocks for each test
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new UpdateUserHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesUserAndReturnsUserDTO()
        {
            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(),
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "abc123abc323"
            };

            var user = new User(command.Name, command.Email, command.Password);
            var userDto = new UserDTO { Id = user.Id, Name = command.Name, Email = command.Email };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.UpdateAsync(user));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(userDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDto.Id, result.Id);
            Assert.Equal(userDto.Name, result.Name);
            Assert.Equal(userDto.Email, result.Email);
            Assert.True(user.VerifyPassword(command.Password), user.PasswordHash);
        }

        // Test exception throwing when request is null
        [Fact]
        public async Task VerifyRequest_ThrowsException_WhenRequestIsNull()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.Handle(null!, CancellationToken.None)
            );

            Assert.Equal("request", exception.ParamName);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(),
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "abc123abc123"
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.Id} not found", exception.Message);
        }

        // Test exception throwing when an email already exists
        [Fact]
        public async Task VerifyUserEmail_ThrowsException_WhenUserEmailAlreadyExists()
        {
            var existingUser = new User("Daniel", "d@gmail.com", "abc123abc323");

            var command = new UpdateUserCommand
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Email = "d2@gmail.com",
                Password = existingUser.PasswordHash,
            };

            _mockUserRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(existingUser);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A user with email '{command.Email}' already exists.", exception.Message);
        }
    }
}
