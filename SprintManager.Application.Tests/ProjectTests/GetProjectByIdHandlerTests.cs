using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Projects;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Projects;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectTests
{
    public class GetProjectByIdHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProjectByIdHandler _handler;

        public GetProjectByIdHandlerTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetProjectByIdHandler(_mockProjectRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsProjectDTO()
        {
            var query = new GetProjectByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var project = new Project("Recipe Forum", "Forum where users can share recipes");

            var projectDTO = new ProjectDTO 
            { 
                Id = project.Id, 
                Name = project.Name, 
                Description = project.Description, 
                CreationDate = project.CreationDate, 
                Status = project.Status 
            };

            // Repository's mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(project);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<ProjectDTO>(project)).Returns(projectDTO);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(projectDTO.Id, result.Id);
            Assert.Equal(projectDTO.Name, result.Name);
            Assert.Equal(projectDTO.Description, result.Description);
            Assert.Equal(projectDTO.CreationDate, result.CreationDate);
            Assert.Equal(projectDTO.Status, result.Status);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<ProjectDTO>(project), Times.Once);
        }

        // Test exception throwing when project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var query = new GetProjectByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {query.Id} not found.", exception.Message);
        }
    }
}