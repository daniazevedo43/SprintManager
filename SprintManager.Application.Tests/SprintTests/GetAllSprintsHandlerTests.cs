using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.SprintTests
{
    public class GetAllSprintsHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllSprintsHandler _handler;

        public GetAllSprintsHandlerTests()
        {
            // Initialize mocks for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new GetAllSprintsHandler(_mockSprintRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllSprints()
        {
            var query = new GetAllSprintsQuery();

            var sprints = new List<Sprint>()
            {
                new Sprint(Guid.NewGuid(), "Sprint 1", new DateTime(2025, 6, 3), new DateTime(2025, 6, 4)),
                new Sprint(Guid.NewGuid(), "Sprint 2", new DateTime(2025, 6, 5), new DateTime(2025, 6, 6)),
            };

            var sprintsDTOs = new List<SprintDTO>()
            {
                new SprintDTO
                {
                    Id = sprints[0].Id,
                    ProjectId = sprints[0].ProjectId,
                    Name = sprints[0].Name,
                    StartDate = sprints[0].StartDate,
                    EndDate = sprints[0].EndDate,
                    Status = sprints[0].Status,
                },
                new SprintDTO
                {
                    Id = sprints[1].Id,
                    ProjectId = sprints[1].ProjectId,
                    Name = sprints[1].Name,
                    StartDate = sprints[1].StartDate,
                    EndDate = sprints[1].EndDate,
                    Status = sprints[1].Status,
                },
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(sprints);

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<SprintDTO>>(sprints)).Returns(sprintsDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < sprintsDTOs.Count; i++)
            {
                Assert.Equal(sprintsDTOs[i].Id, result[i].Id);
                Assert.Equal(sprintsDTOs[i].ProjectId, result[i].ProjectId);
                Assert.Equal(sprintsDTOs[i].Name, result[i].Name);
                Assert.Equal(sprintsDTOs[i].StartDate, result[i].StartDate);
                Assert.Equal(sprintsDTOs[i].EndDate, result[i].EndDate);
                Assert.Equal(sprintsDTOs[i].Description, result[i].Description);
                Assert.Equal(sprintsDTOs[i].Status, result[i].Status);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<SprintDTO>>(sprints), Times.Once);
        }
    }
}