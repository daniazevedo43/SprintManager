using Moq;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.Handlers.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class DeleteWorkItemHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly DeleteWorkItemHandler _handler;

        public DeleteWorkItemHandlerTests()
        {
            // Initialize mock for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();

            // Initialize handler injecting the mock
            _handler = new DeleteWorkItemHandler(_mockWorkItemRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesWorkItem()
        {
            var command = new DeleteWorkItemCommand
            {
                Id = Guid.NewGuid(),
            };

            var workItem = new WorkItem(Guid.NewGuid(), "Adjust feed page for mobile devices", WorkItemType.Task);

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(workItem);
            _mockWorkItemRepository.Setup(r => r.DeleteAsync(workItem));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.DeleteAsync(workItem), Times.Once);
        }

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            var command = new DeleteWorkItemCommand
            {
                Id = Guid.NewGuid(),
            };

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {command.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
}