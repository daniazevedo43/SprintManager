using Moq;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.Handlers.Projects;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectTests
{
    public class DeleteProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly DeleteProjectHandler _handler;

        public DeleteProjectHandlerTests()
        {
            // Initialize mock for each test
            _mockProjectRepository = new Mock<IProjectRepository>();

            // Initialize hanlder injecting the mock
            _handler = new DeleteProjectHandler(_mockProjectRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesProject()
        {
            var command = new DeleteProjectCommand
            {
                Id = Guid.NewGuid(),
            };

            var project = new Project("Recipe Forum", "Forum where users can share recipes");

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.DeleteAsync(project));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.DeleteAsync(project), Times.Once);
        }

        // Test exception throwing when project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var command = new DeleteProjectCommand
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command.Id} not found.", exception.Message);
        }
    }
}