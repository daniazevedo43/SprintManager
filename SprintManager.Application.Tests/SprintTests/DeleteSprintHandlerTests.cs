using Moq;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.SprintTests
{
    public class DeleteSprintHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly DeleteSprintHandler _handler;

        public DeleteSprintHandlerTests()
        {
            // Initialize mock for each test
            _mockSprintRepository = new Mock<ISprintRepository>();

            // Initialize hanlder injecting the mock
            _handler = new DeleteSprintHandler(_mockSprintRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesSprint()
        {
            var command = new DeleteSprintCommand
            {
                Id = Guid.NewGuid(),
            };

            var sprint = new Sprint(Guid.NewGuid(), "Sprint 1", new DateTime(2025, 8, 4), new DateTime(2025, 8, 11));

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(sprint);
            _mockSprintRepository.Setup(r => r.DeleteAsync(sprint));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.DeleteAsync(sprint), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifySprint_ThrowsException_WhenSprintIsNotFound()
        {
            var command = new DeleteSprintCommand
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.Id} not found.", exception.Message);
        }
    }
}