using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using System.Security.Claims;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class CreateWorkItemHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateWorkItemHandler _handler;

        public CreateWorkItemHandlerTests()
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
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
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
            _handler = new CreateWorkItemHandler(
                _mockHttpContextAccessor.Object,
                _mockWorkItemRepository.Object, 
                _mockProjectRepository.Object,
                _mockSprintRepository.Object,
                _mockUserManager.Object,
                _mockMapper.Object
            );
        }

        // Test handler - basic work item creation
        [Fact]
        public async Task Handle_CreatesBasicWorkItem_ReturnsWorkItemDto()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
            };

            var creatorUserId = Guid.NewGuid();

            var workItem = new WorkItem(
                command.ProjectId, 
                command.WorkItemTitle, 
                command.WorkItemType,
                creatorUserId
            );

            var workItemDto = new WorkItemDto
            {
                Id = workItem.Id,
                ProjectId = workItem.ProjectId,
                WorkItemTitle = workItem.WorkItemTitle,
                WorkItemType = workItem.WorkItemType,
                Status = workItem.Status,
                PriorityLevel = workItem.PriorityLevel,
                CreationDate = workItem.CreationDate,
            };

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, creatorUserId.ToString()));

            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockWorkItemRepository.Setup(r => r.AddAsync(It.IsAny<WorkItem>())).Callback<WorkItem>(w => workItem = w);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<WorkItemDto>(It.IsAny<WorkItem>())).Returns(workItemDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workItemDto.Id, result.Id);
            Assert.Equal(workItemDto.ProjectId, result.ProjectId);
            Assert.Equal(workItemDto.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDto.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDto.Status, result.Status);
            Assert.Equal(workItemDto.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDto.CreationDate, result.CreationDate);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(It.IsAny<WorkItem>()), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created work item.
            _mockMapper.Verify(m => m.Map<WorkItemDto>(workItem), Times.Once);
        }

        // Test handler - full work item creation
        [Fact]
        public async Task Handle_CreatesFullWorkItem_ReturnsWorkItemDto()
        {
            var command = new CreateWorkItemCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintId = Guid.NewGuid(),
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task,
                Description = "Test description",
                PriorityLevel = WorkItemPriorityLevel.Medium,
                CompletionDate = DateTime.UtcNow.ToUniversalTime().AddDays(1),
                HoursEstimate = 6
            };

            var creatorUserId = Guid.NewGuid();

            var workItem = new WorkItem(
                command.ProjectId,
                command.WorkItemTitle,
                command.WorkItemType,
                command.SprintId,
                command.AssignedUserId,
                creatorUserId,
                command.Description,
                command.PriorityLevel,
                command.CompletionDate,
                command.HoursEstimate
            );

            var workItemDto = new WorkItemDto
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

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, creatorUserId.ToString()));

            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.AssignedUserId.Value.ToString()!)).ReturnsAsync(new User());
            _mockWorkItemRepository.Setup(r => r.AddAsync(It.IsAny<WorkItem>())).Callback<WorkItem>(w => workItem = w);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<WorkItemDto>(It.IsAny<WorkItem>())).Returns(workItemDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(workItemDto.Id, result.Id);
            Assert.Equal(workItemDto.ProjectId, result.ProjectId);
            Assert.Equal(workItemDto.SprintId, result.SprintId);
            Assert.Equal(workItemDto.AssignedUserId, result.AssignedUserId);
            Assert.Equal(workItemDto.CreatorUserId, result.CreatorUserId);
            Assert.Equal(workItemDto.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDto.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDto.Description, result.Description);
            Assert.Equal(workItemDto.Status, result.Status);
            Assert.Equal(workItemDto.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDto.CreationDate, result.CreationDate);
            Assert.Equal(workItemDto.CompletionDate, result.CompletionDate);
            Assert.Equal(workItemDto.HoursEstimate, result.HoursEstimate);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(command.AssignedUserId.Value.ToString()!), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.AddAsync(workItem), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created work item.
            _mockMapper.Verify(m => m.Map<WorkItemDto>(workItem), Times.Once);
        }

        // Test exception throwing when user is not authenticated
        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new CreateWorkItemCommand();

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier));

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not authenticated.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);
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

            var creatorUserId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, creatorUserId.ToString()));

            // Repository's mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command.ProjectId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

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

            var creatorUserId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, creatorUserId.ToString()));

            // Repositories mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.SprintId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

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
                AssignedUserId = Guid.NewGuid(),
                WorkItemTitle = "Test title",
                WorkItemType = WorkItemType.Task
            };

            var creatorUserId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, creatorUserId.ToString()));

            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(new Sprint());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.AssignedUserId.Value.ToString()!));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.AssignedUserId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(command.AssignedUserId.Value.ToString()!), Times.Once);
        }
    }
}