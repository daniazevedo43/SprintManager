using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.SprintTests
{
    public class CreateSprintHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateSprintHandler _handler;

        public CreateSprintHandlerTests()
        {
            // Initialize mocks for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateSprintHandler(
                _mockSprintRepository.Object, 
                _mockProjectRepository.Object,
                _mockMapper.Object
            );
        }

        // Test handler - project creation without description
        [Fact]
        public async Task Handle_CreatesSprintWithoutDescription_ReturnsSprintDto()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintName = "Test",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20)
            };

            var sprint = new Sprint(command.ProjectId, command.SprintName, command.StartDate, command.EndDate);
            var sprintDto = new SprintDto
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                SprintName = sprint.SprintName,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Status = sprint.Status
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName)).ReturnsAsync((Sprint?)null);
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.AddAsync(It.IsAny<Sprint>())).Callback<Sprint>(s => sprint = s);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<SprintDto>(It.IsAny<Sprint>())).Returns(sprintDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(sprintDto.Id, result.Id);
            Assert.Equal(sprintDto.ProjectId, result.ProjectId);
            Assert.Equal(sprintDto.SprintName, result.SprintName);
            Assert.Equal(sprintDto.StartDate, result.StartDate);
            Assert.Equal(sprintDto.EndDate, result.EndDate);
            Assert.Null(result.Description);
            Assert.Equal(sprintDto.Status, result.Status);

            // Ensure GetByProjectIdAndNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.AddAsync(It.IsAny<Sprint>()), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<SprintDto>(sprint), Times.Once);
        }

        // Test handler - project creation with description
        [Fact]
        public async Task Handle_CreatesSprintWithDescription_ReturnsSprintDto()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintName = "Test",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20),
                Description = "Test description"
            };

            var sprint = new Sprint(command.ProjectId, command.SprintName, command.StartDate, command.EndDate, command.Description);
            var sprintDto = new SprintDto
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                SprintName = sprint.SprintName,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Description = sprint.Description,
                Status = sprint.Status
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName)).ReturnsAsync((Sprint?)null);
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockSprintRepository.Setup(r => r.AddAsync(It.IsAny<Sprint>())).Callback<Sprint>(s => sprint = s);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<SprintDto>(It.IsAny<Sprint>())).Returns(sprintDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(sprintDto.Id, result.Id);
            Assert.Equal(sprintDto.ProjectId, result.ProjectId);
            Assert.Equal(sprintDto.SprintName, result.SprintName);
            Assert.Equal(sprintDto.StartDate, result.StartDate);
            Assert.Equal(sprintDto.EndDate, result.EndDate);
            Assert.Equal(sprintDto.Description, result.Description);
            Assert.Equal(sprintDto.Status, result.Status);

            // Ensure GetByProjectIdAndNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.AddAsync(sprint), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<SprintDto>(sprint), Times.Once);
        }

        // Test exception throwing when a sprint's name already exists
        [Fact]
        public async Task VerifySprintName_ThrowsException_WhenSprintNameAlreadyExists()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintName = "Test",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20)
            };

            var sprint = new Sprint(command.ProjectId, command.SprintName, command.StartDate, command.EndDate);

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName)).ReturnsAsync(sprint);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A sprint called '{command.SprintName}' already exists in this project.", exception.Message);

            // Ensure GetByProjectIdAndNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName), Times.Once);
        }

        // Test exception throwing when a project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                SprintName = "Test",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20)
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName));
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command.ProjectId} not found.", exception.Message);

            // Ensure GetByProjectIdAndNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByProjectIdAndSprintNameAsync(command.ProjectId, command.SprintName), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
        }
    }
}