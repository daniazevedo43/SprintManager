using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateWorkItemHandler _handler;

        public UpdateWorkItemHandlerTests()
        {
            // Create mocks for UserManager constructor's dependencies
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockOptions = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var mockUserValidator = new List<IUserValidator<User>>
            {
                new Mock<IUserValidator<User>>().Object
            };
            var mockPasswordValidator = new List<IPasswordValidator<User>>
            {
                new Mock<IPasswordValidator<User>>().Object
            };
            var mockLookupNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<User>>>();

            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                mockOptions.Object,
                mockPasswordHasher.Object,
                mockUserValidator,
                mockPasswordValidator,
                mockLookupNormalizer.Object,
                mockErrors.Object,
                mockServiceProvider.Object,
                mockLogger.Object
            );
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new UpdateWorkItemHandler(
                _mockWorkItemRepository.Object,
                _mockSprintRepository.Object,
                _mockUserManager.Object,
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesWorkItem_ReturnsWorkItemDTO()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            var workItem = new WorkItem(
                Guid.NewGuid(), command.WorkItemTitle, command.WorkItemType,
                command.SprintId, command.AssignedUserId, Guid.NewGuid(), 
                command.Description, command.PriorityLevel, 
                command.CompletionDate, command.HoursEstimate
            );

            var workItemDTO = new WorkItemDTO
            {
                Id = workItem.Id,
                ProjectId = workItem.ProjectId,
                SprintId = workItem.SprintId,
                AssignedUserId = workItem.AssignedUserId,
                CreatorUserId = workItem.CreatorUserId,
                WorkItemTitle = workItem.WorkItemTitle,
                WorkItemType = workItem.WorkItemType,
                Description = workItem.Description,
                Status = workItem.Status,
                PriorityLevel = workItem.PriorityLevel,
                CreationDate = workItem.CreationDate,
                CompletionDate = workItem.CompletionDate,
                HoursEstimate = workItem.HoursEstimate,
            };

            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.AssignedUserId.ToString()!)).ReturnsAsync(new User());
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(workItem);
            _mockWorkItemRepository.Setup(r => r.UpdateAsync(workItem));

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<WorkItemDTO>(workItem)).Returns(workItemDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workItemDTO.Id, result.Id);
            Assert.Equal(workItemDTO.ProjectId, result.ProjectId);
            Assert.Equal(workItemDTO.SprintId, result.SprintId);
            Assert.Equal(workItemDTO.AssignedUserId, result.AssignedUserId);
            Assert.Equal(workItemDTO.CreatorUserId, result.CreatorUserId);
            Assert.Equal(workItemDTO.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDTO.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDTO.Description, result.Description);
            Assert.Equal(workItemDTO.Status, result.Status);
            Assert.Equal(workItemDTO.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDTO.CreationDate, result.CreationDate);
            Assert.Equal(workItemDTO.CompletionDate, result.CompletionDate);
            Assert.Equal(workItemDTO.HoursEstimate, result.HoursEstimate);

            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockUserManager.Verify(m => m.FindByIdAsync(command.AssignedUserId.ToString()!), Times.Once);
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockWorkItemRepository.Verify(r => r.UpdateAsync(workItem), Times.Once);
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifySprint_ThrowsException_WhenSprintIsNotFound()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.SprintId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.AssignedUserId.ToString()!));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.AssignedUserId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(command.AssignedUserId.ToString()!), Times.Once);
        }

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            var command = new UpdateWorkItemCommand
            {
                Id = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
                Status = WorkItemStatus.Active,
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.AssignedUserId.ToString()!)).ReturnsAsync(new User());
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {command.Id} not found.", exception.Message);

            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockUserManager.Verify(m => m.FindByIdAsync(command.AssignedUserId.ToString()!), Times.Once);
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
}