using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.ProjectMembers;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Tests.ProjectMemberTests
{
    public class GetAllProjectMembersHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllProjectMembersHandler _handler;

        public GetAllProjectMembersHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize hanlder injecting the mocks
            _handler = new GetAllProjectMembersHandler(_mockProjectMemberRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllProjectMembers()
        {
            var query = new GetAllProjectMembersQuery();

            var projectMembers = new List<ProjectMember>()
            {
                new ProjectMember(Guid.NewGuid(), Guid.NewGuid(), ProjectMemberRole.Developer),
                new ProjectMember(Guid.NewGuid(), Guid.NewGuid(), ProjectMemberRole.Client)
            };

            var projectMembersDTOs = new List<ProjectMemberDTO>()
            {
                new ProjectMemberDTO
                {
                    Id = projectMembers[0].Id,
                    ProjectId = projectMembers[0].ProjectId,
                    UserId = projectMembers[0].UserId,
                    Role = projectMembers[0].Role,
                },
                new ProjectMemberDTO
                {
                    Id = projectMembers[1].Id,
                    ProjectId = projectMembers[1].ProjectId,
                    UserId = projectMembers[1].UserId,
                    Role = projectMembers[1].Role,
                }
            };

            // Repository's Mock configuration
            _mockProjectMemberRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(projectMembers);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<ProjectMemberDTO>>(projectMembers)).Returns(projectMembersDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < projectMembersDTOs.Count; i++)
            {
                Assert.Equal(projectMembersDTOs[i].Id, result[i].Id);
                Assert.Equal(projectMembersDTOs[i].ProjectId, result[i].ProjectId);
                Assert.Equal(projectMembersDTOs[i].UserId, result[i].UserId);
                Assert.Equal(projectMembersDTOs[i].Role, result[i].Role);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<ProjectMemberDTO>>(projectMembers), Times.Once);
        }
    }
}