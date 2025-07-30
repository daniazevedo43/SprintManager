using AutoMapper;
using Moq;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class CreateWorkItemHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateWorkItemHandler _handler;

        public CreateWorkItemHandlerTests()
        {
            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateWorkItemHandler(_mockWorkItemRepository.Object, _mockMapper.Object);
        }

        // Test handler - basic work item creation
        [Fact]
        public async Task Handle_CreatesBasicWorkItem_ReturnsWorkItemDTO()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                WorkItemTitle = "Adjust feed page for mobile devices",
                WorkItemType = WorkItemType.Task,
            };

            var workItem = new WorkItem(command.ProjectId, command.WorkItemTitle, command.WorkItemType);
            var workItemDTO = new WorkItemDTO
            {
                Id = workItem.Id,
                ProjectId = workItem.ProjectId,
                WorkItemTitle = workItem.WorkItemTitle,
                WorkItemType = workItem.WorkItemType,
                Status = workItem.Status,
                PriorityLevel = workItem.PriorityLevel,
                CreationDate = workItem.CreationDate,
            };

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(r => r.AddAsync(It.IsAny<WorkItem>())).Callback<WorkItem>(w => workItem = w);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<WorkItemDTO>(It.IsAny<WorkItem>())).Returns(workItemDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workItemDTO.Id, result.Id);
            Assert.Equal(workItemDTO.ProjectId, result.ProjectId);
            Assert.Equal(workItemDTO.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDTO.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDTO.Status, result.Status);
            Assert.Equal(workItemDTO.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDTO.CreationDate, result.CreationDate);

            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(workItem), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created work item.
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }

        // Test handler - full work item creation
        [Fact]
        public async Task Handle_CreatesFullWorkItem_ReturnsWorkItemDTO()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WorkItemTitle = "Adjust feed page for mobile devices",
                WorkItemType = WorkItemType.Task,
                Description = "The feed page needs to be responsive for mobile devices.",
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            var workItem = new WorkItem(
                command.ProjectId,
                command.WorkItemTitle, 
                command.WorkItemType,
                command.SprintId,
                command.UserId,
                command.Description,
                command.PriorityLevel,
                command.CompletionDate,
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
            _mockWorkItemRepository.Setup(r => r.AddAsync(It.IsAny<WorkItem>())).Callback<WorkItem>(w => workItem = w);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<WorkItemDTO>(It.IsAny<WorkItem>())).Returns(workItemDTO);

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

            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(workItem), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created work item.
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }
    }
}