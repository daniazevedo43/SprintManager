using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Tests.SprintTests
{
    public class CreateSprintHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateSprintHandler _handler;

        public CreateSprintHandlerTests()
        {
            // Initialize mocks for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new CreateSprintHandler(_mockSprintRepository.Object, _mockMapper.Object);
        }

        // Test handler - project creation without description
        [Fact]
        public async Task Handle_CreatesSprintWithoutDescription_ReturnsSprintDTO()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                Name = "Sprint 1",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20),
                Status = SprintStatus.Active
            };

            var sprint = new Sprint(command.ProjectId, command.Name, command.StartDate, command.EndDate);
            var sprintDTO = new SprintDTO
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Status = sprint.Status
            };

            // Repositories mock configuration
            _mockSprintRepository.Setup(r => r.GetByNameAsync(sprint.Name)).ReturnsAsync((Sprint?)null);
            _mockSprintRepository.Setup(r => r.AddAsync(It.IsAny<Sprint>())).Callback<Sprint>(s => sprint = s);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<SprintDTO>(It.IsAny<Sprint>())).Returns(sprintDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(sprintDTO.Id, result.Id);
            Assert.Equal(sprintDTO.ProjectId, result.ProjectId);
            Assert.Equal(sprintDTO.Name, result.Name);
            Assert.Equal(sprintDTO.StartDate, result.StartDate);
            Assert.Equal(sprintDTO.EndDate, result.EndDate);
            Assert.Equal(sprintDTO.Status, result.Status);

            // Ensure GetByNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByNameAsync(sprint.Name), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.AddAsync(sprint), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<SprintDTO>(sprint), Times.Once);
        }

        // Test handler - project creation with description
        [Fact]
        public async Task Handle_CreatesSprintWithDescription_ReturnsSprintDTO()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                Name = "Sprint 1",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20),
                Description = "Project setup and authentication",
                Status = SprintStatus.Active
            };

            var sprint = new Sprint(command.ProjectId, command.Name, command.StartDate, command.EndDate, command.Description);
            var sprintDTO = new SprintDTO
            {
                Id = sprint.Id,
                ProjectId = sprint.ProjectId,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Description = sprint.Description,
                Status = sprint.Status
            };

            // Repositories mock configuration
            _mockSprintRepository.Setup(r => r.GetByNameAsync(sprint.Name)).ReturnsAsync((Sprint?)null);
            _mockSprintRepository.Setup(r => r.AddAsync(It.IsAny<Sprint>())).Callback<Sprint>(s => sprint = s);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<SprintDTO>(It.IsAny<Sprint>())).Returns(sprintDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(sprintDTO.Id, result.Id);
            Assert.Equal(sprintDTO.ProjectId, result.ProjectId);
            Assert.Equal(sprintDTO.Name, result.Name);
            Assert.Equal(sprintDTO.StartDate, result.StartDate);
            Assert.Equal(sprintDTO.EndDate, result.EndDate);
            Assert.Equal(sprintDTO.Description, result.Description);
            Assert.Equal(sprintDTO.Status, result.Status);

            // Ensure GetByNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByNameAsync(sprint.Name), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.AddAsync(sprint), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created project.
            _mockMapper.Verify(m => m.Map<SprintDTO>(sprint), Times.Once);
        }

        //// Test exception throwing when a project already exists
        [Fact]
        public async Task VerifySprintName_ThrowsException_WhenSprintNameAlreadyExists()
        {
            var command = new CreateSprintCommand
            {
                ProjectId = Guid.NewGuid(),
                Name = "Sprint 1",
                StartDate = new DateTime(2025, 1, 6),
                EndDate = new DateTime(2025, 1, 20),
                Description = "Project setup and authentication",
                Status = SprintStatus.Active
            };

            var sprint = new Sprint(command.ProjectId, command.Name, command.StartDate, command.EndDate);

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByNameAsync(command.Name)).ReturnsAsync(sprint);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A sprint called '{command.Name}' already exists.", exception.Message);

            // Ensure GetByNameAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByNameAsync(command.Name), Times.Once);
        }
    }
}