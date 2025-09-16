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
    public class CreateWorkItemHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateWorkItemHandler _handler;

        public CreateWorkItemHandlerTests()
        {
            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateWorkItemHandler(
                _mockWorkItemRepository.Object, 
                _mockProjectRepository.Object,
                _mockSprintRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object
            );
        }

        // Test handler - basic work item creation
        [Fact]
        public async Task Handle_CreatesBasicWorkItem_ReturnsWorkItemDTO()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
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

            // Repositories mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync(new User());
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

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.UserId), Times.Once);
           
            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(It.IsAny<WorkItem>()), Times.Once);

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
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
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

            // Repositories mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync(new User());
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

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.UserId), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(workItem), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created work item.
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }

        // Test exception throwing when project is not found
        [Fact]
        public async Task VerifyProjectId_ThrowsException_WhenProjectIsNotFound()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
            };

            // Repository's mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command.ProjectId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifySprintId_ThrowsException_WhenSprintIsNotFound()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
            };

            // Repositories mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.SprintId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifyUserId_ThrowsException_WhenUserIsNotFound()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
            };

            // Repositories mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserRepository.Setup(r => r.GetByIdAsync(command.UserId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.UserId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockUserRepository.Verify(r => r.GetByIdAsync(command.UserId), Times.Once);
        }
    }
}