using AutoMapper;
using Moq;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectMemberTests
{
    public class UpdateProjectMemberRoleHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProjectMemberRoleHandler _handler;
        
        public UpdateProjectMemberRoleHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new UpdateProjectMemberRoleHandler(_mockProjectMemberRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesProjectMemberRole_ReturnsProjectMemberBasicDTO()
        {
            var command = new UpdateProjectMemberRoleCommand
            {
                Id = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(Guid.NewGuid(), Guid.NewGuid(), command.Role);
            var projectMemberDTO = new ProjectMemberDTO
            {
                Id = command.Id,
                ProjectId = projectMember.ProjectId,
                UserId = projectMember.UserId,
                Role = command.Role
            };

            // Repositories Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(projectMember);
            _mockProjectMemberRepository.Setup(r => r.UpdateAsync(projectMember));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<ProjectMemberDTO>(projectMember)).Returns(projectMemberDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectMemberDTO.Id, result.Id);
            Assert.Equal(projectMemberDTO.ProjectId, result.ProjectId);
            Assert.Equal(projectMemberDTO.UserId, result.UserId);
            Assert.Equal(projectMemberDTO.Role, result.Role);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockProjectMemberRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure UpdateAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.UpdateAsync(projectMember), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified project's member.
            _mockMapper.Verify(m => m.Map<ProjectMemberDTO>(projectMember), Times.Once);
        }

        // Test exception throwing when user and project relationship is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenUserAndProjectRelationshipNotFound()
        {
            var command = new UpdateProjectMemberRoleCommand
            {
                Id = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"There's no relationship between a user and a project with ID {command.Id}.", exception.Message);
        }
    }
}