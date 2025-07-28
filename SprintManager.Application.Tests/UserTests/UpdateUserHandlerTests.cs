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

            // Initialize handler injecting the mocks
            _handler = new UpdateUserHandler(_mockUserRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesUser_ReturnsUserDTO()
        {
            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(),
                Name = "Daniel",
                Email = "d@gmail.com",
                Password = "abc123abc323"
            };

            var user = new User(command.Name, command.Email, command.Password);
            var userDTO = new UserDTO { Id = user.Id, Name = command.Name, Email = command.Email };

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _mockUserRepository.Setup(r => r.UpdateAsync(user));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Name, result.Name);
            Assert.Equal(userDTO.Email, result.Email);
            Assert.True(user.VerifyPassword(command.Password), user.PasswordHash);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetByEmailAsync was called exactly once.
            _mockUserRepository.Verify(r => r.GetByEmailAsync(user.Email), Times.Once);

            // Ensure UpdateAsync was called exactly once with the modified user.
            _mockUserRepository.Verify(r => r.UpdateAsync(user), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified user.
            _mockMapper.Verify(m => m.Map<UserDTO>(user), Times.Once);
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

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetByEmailAsync was called exactly once.
            _mockUserRepository.Verify(r => r.GetByEmailAsync(command.Email), Times.Once);
        }
    }
}