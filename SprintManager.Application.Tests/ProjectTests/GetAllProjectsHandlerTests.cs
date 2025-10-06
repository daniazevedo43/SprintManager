using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Projects;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Projects;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.ProjectTests
{
    public class GetAllProjectsHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllProjectsHandler _handler;

        public GetAllProjectsHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllProjectsHandler(_mockProjectRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllProjects()
        {
            var query = new GetAllProjectsQuery();

            var projects = new List<Project>()
            {
                new Project("Test", "Test Description"),
                new Project("Test 2", "Test Description 2")
            };

            var projectsDTOs = new List<ProjectDto>()
            {
                new ProjectDto
                {
                    Id = projects[0].Id,
                    Name = projects[0].Name, 
                    Description = projects[0].Description, 
                    CreationDate = projects[0].CreationDate, 
                    Status = projects[0].Status
                },
                new ProjectDto
                {
                    Id = projects[1].Id,
                    Name = projects[1].Name,
                    Description = projects[1].Description,
                    CreationDate = projects[1].CreationDate,
                    Status = projects[1].Status
                }
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(projects);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<ProjectDto>>(projects)).Returns(projectsDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for(int i = 0; i < projectsDTOs.Count; i++)
            {
                Assert.Equal(projectsDTOs[i].Id, result[i].Id);
                Assert.Equal(projectsDTOs[i].Name, result[i].Name);
                Assert.Equal(projectsDTOs[i].Description, result[i].Description);
                Assert.Equal(projectsDTOs[i].CreationDate, result[i].CreationDate);
                Assert.Equal(projectsDTOs[i].Status, result[i].Status);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<ProjectDto>>(projects), Times.Once);
        }
    }
}