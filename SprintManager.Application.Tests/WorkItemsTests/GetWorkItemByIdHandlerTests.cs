using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.WorkItems;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.WorkItemsTests
{
    public class GetWorkItemByIdHandlerTests
    {
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetWorkItemByIdHandler _handler;

        public GetWorkItemByIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetWorkItemByIdHandler(_mockWorkItemRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsWorkItemDTO()
        {
            var query = new GetWorkItemByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var workItem = new WorkItem(Guid.NewGuid(), "Adjust feed page for mobile devices", WorkItemType.Task);

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
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(workItem);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<WorkItemDTO>(workItem)).Returns(workItemDTO);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(workItemDTO.Id, result.Id);
            Assert.Equal(workItemDTO.ProjectId, result.ProjectId);
            Assert.Equal(workItemDTO.WorkItemTitle, result.WorkItemTitle);
            Assert.Equal(workItemDTO.WorkItemType, result.WorkItemType);
            Assert.Equal(workItemDTO.Status, result.Status);
            Assert.Equal(workItemDTO.PriorityLevel, result.PriorityLevel);
            Assert.Equal(workItemDTO.CreationDate, result.CreationDate);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockWorkItemRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<WorkItemDTO>(workItem), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            var query = new GetWorkItemByIdQuery
            {
                Id = Guid.NewGuid()
            };

            // Repository's mock configuration
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(query.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {query.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);
        }
    }
}