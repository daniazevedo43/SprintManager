using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.SprintTests
{
    public class GetSprintByIdHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetSprintByIdHandler _handler;

        public GetSprintByIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetSprintByIdHandler(_mockSprintRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsSprintDTO()
        {
            var query = new GetSprintByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var sprint = new Sprint(Guid.NewGuid(), "Sprint 1", new DateTime(2025, 6, 3), new DateTime(2025, 6, 4));

            var sprintDTO = new SprintDTO
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                SprintName = sprint.SprintName,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Status = sprint.Status,
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(sprint);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<SprintDTO>(sprint)).Returns(sprintDTO);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(sprintDTO.Id, result.Id);
            Assert.Equal(sprintDTO.ProjectId, result.ProjectId);
            Assert.Equal(sprintDTO.SprintName, result.SprintName);
            Assert.Equal(sprintDTO.StartDate, result.StartDate);
            Assert.Equal(sprintDTO.EndDate, result.EndDate);
            Assert.Equal(sprintDTO.Status, result.Status);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockSprintRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<SprintDTO>(sprint), Times.Once);
        }

        // Test exception throwing when sprint is not found
        [Fact]
        public async Task VerifySprint_ThrowsException_WhenSprintIsNotFound()
        {
            var query = new GetSprintByIdQuery
            {
                Id = Guid.NewGuid()
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(query.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {query.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);
        }
    }
}