using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Projects;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectTests
{
    public class UpdateProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProjectHandler _handler;

        public UpdateProjectHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new UpdateProjectHandler(_mockProjectRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesProject_ReturnsProjectDTO()
        {
            var command = new UpdateProjectCommand
            {
                Id = Guid.NewGuid(),
                Name = "Recipe Forum",
                Description = "Forum where users can share recipes",
                Status = ProjectStatus.Completed
            };

            var project = new Project(command.Name, command.Description);
            var projectDTO = new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = command.Status
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.UpdateAsync(project));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<ProjectDTO>(project)).Returns(projectDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectDTO.Id, result.Id);
            Assert.Equal(projectDTO.Name, result.Name);
            Assert.Equal(projectDTO.Description, result.Description);
            Assert.Equal(projectDTO.CreationDate, result.CreationDate);
            Assert.Equal(projectDTO.Status, result.Status);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetByNameAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByNameAsync(project.Name), Times.Once);

            // Ensure UpdateAsync was called exactly once with the modified project.
            _mockProjectRepository.Verify(r => r.UpdateAsync(project), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified project.
            _mockMapper.Verify(m => m.Map<ProjectDTO>(project), Times.Once);
        }

        // Test exception throwing when project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var command = new UpdateProjectCommand
            {
                Id = Guid.NewGuid(),
                Name = "Recipe Forum",
                Description = "Forum where users can share recipes",
                Status = ProjectStatus.Completed
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command?.Id} not found", exception.Message);
        }

        // Test exception throwing when a project already exists
        [Fact]
        public async Task VerifyProjectName_ThrowsException_WhenProjectNameAlreadyExists()
        {
            var existingProject = new Project("Recipe Forum", "Forum where users can share recipes");

            var command = new UpdateProjectCommand
            {
                Id = Guid.NewGuid(),
                Name = "Recipe Forum 2",
                Description = existingProject.Description,
                Status = existingProject.Status
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingProject);
            _mockProjectRepository.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync(existingProject);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A project called '{command.Name}' already exists.", exception.Message);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure GetByNameAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByNameAsync(command.Name), Times.Once);
        }
    }
}