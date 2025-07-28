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
    public class GetProjectMembersByProjectIdHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProjectMembersByProjectIdHandler _handler;

        public GetProjectMembersByProjectIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetProjectMembersByProjectIdHandler(_mockProjectMemberRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsProjectMemberBasicDTO()
        {
            var query = new GetProjectMembersByProjectIdQuery()
            {
                ProjectId = Guid.NewGuid()
            };

            var projectMembers = new List<ProjectMember>()
            {
                new ProjectMember(query.ProjectId, Guid.NewGuid(), ProjectMemberRole.Developer),
                new ProjectMember(query.ProjectId, Guid.NewGuid(), ProjectMemberRole.Client)
            };

            var projectMembersDTOs = new List<ProjectMemberBasicDTO>()
            {
                new ProjectMemberBasicDTO
                {
                    UserId = projectMembers[0].UserId,
                    Role = projectMembers[0].Role,
                },
                new ProjectMemberBasicDTO
                {
                    UserId = projectMembers[1].UserId,
                    Role = projectMembers[1].Role,
                }
            };

            // Repository's Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetMembersByProjectIdAsync(query.ProjectId)).ReturnsAsync(projectMembers);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<ProjectMemberBasicDTO>>(projectMembers)).Returns(projectMembersDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < projectMembersDTOs.Count; i++)
            {
                Assert.Equal(projectMembersDTOs[i].UserId, result[i].UserId);
                Assert.Equal(projectMembersDTOs[i].Role, result[i].Role);
            }

            // Ensure GetMembersByProjectIdAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetMembersByProjectIdAsync(query.ProjectId), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<ProjectMemberBasicDTO>>(projectMembers), Times.Once);
        }
    }
}