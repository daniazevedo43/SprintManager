using AutoMapper;
using Moq;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectMemberTests
{
    public class RemoveProjectMemberHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RemoveProjectMemberHandler _handler;

        public RemoveProjectMemberHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new RemoveProjectMemberHandler(_mockProjectMemberRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_RemovesUserFromProject()
        {
            var command = new RemoveProjectMemberCommand
            {
                Id = Guid.NewGuid(),
            };

            var projectMember = new ProjectMember(Guid.NewGuid(), Guid.NewGuid(), ProjectMemberRole.Developer);

            // Repository's Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(projectMember);
            _mockProjectMemberRepository.Setup(r => r.DeleteAsync(projectMember));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectMemberRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.DeleteAsync(projectMember), Times.Once);
        }

        // Test exception throwing when user and project relationship is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenUserAndProjectRelationshipNotFound()
        {
            var command = new RemoveProjectMemberCommand
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"There's no relationship between a user and a project with ID {command.Id}.", exception.Message);
        }
    }
}