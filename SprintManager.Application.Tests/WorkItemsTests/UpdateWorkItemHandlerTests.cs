using AutoMapper;
using Moq;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class UpdateWorkItemHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateWorkItemHandler _handler;

        public UpdateWorkItemHandlerTests()
        {
            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new UpdateWorkItemHandler(_mockWorkItemRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesWorkItem_ReturnsWorkItemDTO()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WorkItemTitle = "Adjust feed page for mobile devices",
                WorkItemType = WorkItemType.Task,
                Description = "The feed page needs to be responsive for mobile devices.",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            var workItem = new WorkItem(
                Guid.NewGuid(), command.WorkItemTitle, command.WorkItemType,
                command.SprintId, command.UserId, command.Description,
                command.PriorityLevel, command.CompletionDate, 
                command.HoursEstimate
            );

            var workItemDTO = new WorkItemDTO
            {
                Id = workItem.Id,
                ProjectId = workItem.ProjectId,
                SprintId = workItem.SprintId,
                UserId = workItem.UserId,
                WorkItemTitle = workItem.WorkItemTitle,
                WorkItemType = workItem.WorkItemType,
                Description = workItem.Description,
                Status = workItem.Status,
                PriorityLevel = workItem.PriorityLevel,
                CreationDate = workItem.CreationDate,
                CompletionDate = workItem.CompletionDate,
                HoursEstimate = workItem.HoursEstimate,
            };

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(workItem);
            _mockWorkItemRepository.Setup(r => r.UpdateAsync(workItem));

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<WorkItemDTO>(workItem)).Returns(workItemDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workItemDTO.Id, result.Id);
            Assert.Equal(workItemDTO.ProjectId, result.ProjectId);
            Assert.Equal(workItemDTO.SprintId, result.SprintId);
            Assert.Equal(workItemDTO.UserId, result.UserId);
            Assert.Equal(workItemDTO.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDTO.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDTO.Description, result.Description);
            Assert.Equal(workItemDTO.Status, result.Status);
            Assert.Equal(workItemDTO.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDTO.CreationDate, result.CreationDate);
            Assert.Equal(workItemDTO.CompletionDate, result.CompletionDate);
            Assert.Equal(workItemDTO.HoursEstimate, result.HoursEstimate);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure UpdateAsync was called exactly once with the modified work item.
            _mockWorkItemRepository.Verify(r => r.UpdateAsync(workItem), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified work item.
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WorkItemTitle = "Adjust feed page for mobile devices",
                WorkItemType = WorkItemType.Task,
                Description = "The feed page needs to be responsive for mobile devices.",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {command?.Id} not found.", exception.Message);
        }
    }
}