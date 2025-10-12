using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.ProjectMembers;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectMemberTests
{
    public class GetProjectMembersByProjectIdHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProjectMembersByProjectIdHandler _handler;

        public GetProjectMembersByProjectIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetProjectMembersByProjectIdHandler(
                _mockProjectMemberRepository.Object, 
                _mockProjectRepository.Object,
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsProjectMemberBasicDto()
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

            var projectMembersDtos = new List<ProjectMemberBasicDto>()
            {
                new ProjectMemberBasicDto
                {
                    UserId = projectMembers[0].UserId,
                    Role = projectMembers[0].Role,
                },
                new ProjectMemberBasicDto
                {
                    UserId = projectMembers[1].UserId,
                    Role = projectMembers[1].Role,
                }
            };

            // Repository's Mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(query.ProjectId)).ReturnsAsync(new Project());
            _mockProjectMemberRepository.Setup(r => r.GetMembersByProjectIdAsync(query.ProjectId)).ReturnsAsync(projectMembers);

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<ProjectMemberBasicDto>>(projectMembers)).Returns(projectMembersDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < projectMembersDtos.Count; i++)
            {
                Assert.Equal(projectMembersDtos[i].UserId, result[i].UserId);
                Assert.Equal(projectMembersDtos[i].Role, result[i].Role);
            }

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(query.ProjectId), Times.Once);

            // Ensure GetMembersByProjectIdAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetMembersByProjectIdAsync(query.ProjectId), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<ProjectMemberBasicDto>>(projectMembers), Times.Once);
        }

        // Test exception throwing when project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var query = new GetProjectMembersByProjectIdQuery()
            {
                ProjectId = Guid.NewGuid()
            };

            // Repository's mock configuration
            _mockProjectRepository.Setup(r => r.GetByIdAsync(query.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {query.ProjectId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(query.ProjectId), Times.Once);
        }
    }
}