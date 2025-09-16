using Moq;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.SprintTests
{
    public class DeleteSprintHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly DeleteSprintHandler _handler;

        public DeleteSprintHandlerTests()
        {
            // Initialize mock for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();

            // Initialize handler injecting the mock
            _handler = new DeleteSprintHandler(
                _mockSprintRepository.Object,
                _mockWorkItemRepository.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesSprint()
        {
            var command = new DeleteSprintCommand
            {
                Id = Guid.NewGuid(),
            };

            var sprint = new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 8, 4), new DateTime(2025, 8, 11));

            // Repositories mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(sprint);
            _mockWorkItemRepository.Setup(r => r.GetBySprintIdAsync(command.Id));
            _mockSprintRepository.Setup(r => r.DeleteAsync(sprint));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetBySprintIdAsync was called exactly once with the correct ID.
            _mockWorkItemRepository.Verify(r => r.GetBySprintIdAsync(command.Id), Times.Once);

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

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }

        // Test exception throwing sprint has one or more work items
        [Fact]
        public async Task VerifySprint_ThrowsException_WhenSprintHasWorkItems()
        {
            var command = new DeleteSprintCommand
            {
                Id = Guid.NewGuid(),
            };

            // Repositories mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(new Sprint());
            _mockWorkItemRepository.Setup(r => r.GetBySprintIdAsync(command.Id)).ReturnsAsync(new WorkItem());

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.Id} has one or more work items.", exception.Message);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetBySprintIdAsync was called exactly once with the correct ID.
            _mockWorkItemRepository.Verify(r => r.GetBySprintIdAsync(command.Id), Times.Once);
        }
    }
}