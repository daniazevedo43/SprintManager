using AutoMapper;
using Moq;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using System.Data;

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
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(command.ProjectId, command.UserId, command.Role);
            var projectMemberDTO = new ProjectMemberBasicDTO
            {
                UserId = command.UserId,
                Role = command.Role
            };

            // Repositories Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId)).ReturnsAsync(projectMember);
            _mockProjectMemberRepository.Setup(r => r.UpdateAsync(projectMember));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<ProjectMemberBasicDTO>(projectMember)).Returns(projectMemberDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectMemberDTO.UserId, result.UserId);
            Assert.Equal(projectMemberDTO.Role, result.Role);

            // Ensure GetByUserAndProjectIdAsync was called exactly once with the correct ID.
            _mockProjectMemberRepository.Verify(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId), Times.Once);

            // Ensure UpdateAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.UpdateAsync(projectMember), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified project's member.
            _mockMapper.Verify(m => m.Map<ProjectMemberBasicDTO>(projectMember), Times.Once);
        }
    }
}