using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Projects;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.ProjectTests
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateProjectHandler _handler;

        public CreateProjectHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateProjectHandler(_mockProjectRepository.Object, _mockMapper.Object);
        }

        // Test handler - project creation without description
        [Fact]
        public async Task Handle_CreatesProjectWithoutDescription_ReturnProjectDto()
        {
            var command = new CreateProjectCommand
            {
                Name = "Test",
            };

            var project = new Project(command.Name);
            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreationDate = project.CreationDate,
                Status = project.Status
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByNameAsync(command.Name));
            _mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>())).Callback<Project>(p => project = p);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>())).Returns(projectDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectDto.Id, result.Id);
            Assert.Equal(projectDto.Name, result.Name);
            Assert.Null(result.Description);
            Assert.Equal(projectDto.CreationDate, result.CreationDate);
            Assert.Equal(projectDto.Status, result.Status);

            // Ensure GetByNameAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByNameAsync(command.Name), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.AddAsync(project), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<ProjectDto>(It.IsAny<Project>()), Times.Once);
        }

        // Test handler - project creation with description
        [Fact]
        public async Task Handle_CreatesProjectWithDescription_ReturnsProjectDto()
        {
            var command = new CreateProjectCommand
            {
                Name = "Test",
                Description = "Test Description",
            };

            var project = new Project(command.Name, command.Description);
            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreationDate = project.CreationDate,
                Status = project.Status
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByNameAsync(command.Name));
            _mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>())).Callback<Project>(p => project = p);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>())).Returns(projectDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectDto.Id, result.Id);
            Assert.Equal(projectDto.Name, result.Name);
            Assert.Equal(projectDto.Description, result.Description);
            Assert.Equal(projectDto.CreationDate, result.CreationDate);
            Assert.Equal(projectDto.Status, result.Status);

            // Ensure GetByNameAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByNameAsync(command.Name), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.AddAsync(project), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<ProjectDto>(project), Times.Once);
        }

        // Test exception throwing when a project already exists
        [Fact]
        public async Task VerifyProjectName_ThrowsException_WhenProjectNameAlreadyExists()
        {
            var command = new CreateProjectCommand
            {
                Name = "Test",
                Description = "Test Description",
            };

            var project = new Project(command.Name, command.Description);

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync(project);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A project called '{command.Name}' already exists.", exception.Message);

            // Ensure GetByNameAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByNameAsync(command.Name), Times.Once);
        }
    }
}