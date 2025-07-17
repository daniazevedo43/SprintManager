using Moq;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.Handlers.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.UserTests
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            // Initialize mock for each test
            _mockUserRepository = new Mock<IUserRepository>();

            // Initialize hanlder injecting the mock
            _handler = new DeleteUserHandler(_mockUserRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesUser()
        {
            var command = new DeleteUserCommand
            {
                Id = Guid.NewGuid(),
            };

            var user = new User("Daniel", "d@gmail.com", "abc123abc123");

            // Repository's Mock configuration
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.DeleteAsync(user));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockUserRepository.Verify(r => r.DeleteAsync(user), Times.Once);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new DeleteUserCommand
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.Id} not found.", exception.Message);
        }
    }
}
