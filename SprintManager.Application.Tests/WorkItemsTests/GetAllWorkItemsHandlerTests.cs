using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.WorkItems;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class GetAllWorkItemsHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllWorkItemsHandler _handler;

        public GetAllWorkItemsHandlerTests()
        {
            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllWorkItemsHandler(_mockWorkItemRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllWorkItems()
        {
            var query = new GetAllWorkItemsQuery();

            var creatorUserId = Guid.NewGuid();

            var workItems = new List<WorkItem>()
            {
                new WorkItem(
                    Guid.NewGuid(),
                    "Test title", WorkItemType.Task,
                    creatorUserId, Guid.NewGuid(), Guid.NewGuid(),
                    "Test description",
                    WorkItemPriorityLevel.Low,
                    DateTime.UtcNow.ToUniversalTime().AddDays(1), 8
                ),
                new WorkItem(
                    Guid.NewGuid(), "Test title 2",
                    WorkItemType.Bug, creatorUserId, Guid.NewGuid(),
                    Guid.NewGuid(), "Test description 2",
                    WorkItemPriorityLevel.Low,
                    DateTime.UtcNow.ToUniversalTime().AddDays(1), 8
                )
            };

            var workItemsDtos = new List<WorkItemDto>()
            {
                new WorkItemDto
                {
                    Id = workItems[0].Id,
                    ProjectId = workItems[0].ProjectId,
                    SprintId = workItems[0].SprintId,
                    AssignedUserId = workItems[0].AssignedUserId,
                    CreatorUserId = workItems[0].CreatorUserId,
                    WorkItemTitle = workItems[0].WorkItemTitle,
                    WorkItemType = workItems[0].WorkItemType,
                    Description = workItems[0].Description,
                    Status = workItems[0].Status,
                    PriorityLevel = workItems[0].PriorityLevel,
                    CreationDate = workItems[0].CreationDate,
                    CompletionDate = workItems[0].CompletionDate,
                    HoursEstimate = workItems[0].HoursEstimate,
                },
                new WorkItemDto
                {
                    Id = workItems[1].Id,
                    ProjectId = workItems[1].ProjectId,
                    SprintId = workItems[1].SprintId,
                    AssignedUserId = workItems[1].AssignedUserId,
                    CreatorUserId = workItems[0].CreatorUserId,
                    WorkItemTitle = workItems[1].WorkItemTitle,
                    WorkItemType = workItems[1].WorkItemType,
                    Description = workItems[1].Description,
                    Status = workItems[1].Status,
                    PriorityLevel = workItems[1].PriorityLevel,
                    CreationDate = workItems[1].CreationDate,
                    CompletionDate = workItems[1].CompletionDate,
                    HoursEstimate = workItems[1].HoursEstimate,
                },
            };

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(workItems);

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<WorkItemDto>>(workItems)).Returns(workItemsDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < workItemsDtos.Count; i++)
            {
                Assert.Equal(workItems[i].Id, result[i].Id);
                Assert.Equal(workItems[i].ProjectId, result[i].ProjectId);
                Assert.Equal(workItems[i].SprintId, result[i].SprintId);
                Assert.Equal(workItems[i].AssignedUserId, result[i].AssignedUserId);
                Assert.Equal(workItems[i].CreatorUserId, result[i].CreatorUserId);
                Assert.Equal(workItems[i].WorkItemTitle, result[i].WorkItemTitle);
                Assert.Equal(workItems[i].WorkItemType, result[i].WorkItemType);
                Assert.Equal(workItems[i].Description, result[i].Description);
                Assert.Equal(workItems[i].Status, result[i].Status);
                Assert.Equal(workItems[i].PriorityLevel, result[i].PriorityLevel);
                Assert.Equal(workItems[i].CreationDate, result[i].CreationDate);
                Assert.Equal(workItems[i].CompletionDate, result[i].CompletionDate);
                Assert.Equal(workItems[i].HoursEstimate, result[i].HoursEstimate);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<WorkItemDto>>(workItems), Times.Once);
        }
    }
}