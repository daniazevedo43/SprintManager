using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ProjectMemberTests
{
    public class AddProjectMemberHandlerTests
    {
        private readonly Mock<IProjectMemberRepository> _mockProjectMemberRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddProjectMemberHandler _handler;

        public AddProjectMemberHandlerTests()
        {
            // Create mocks for UserManager constructor's dependencies
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockOptions = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var mockUserValidator = new List<IUserValidator<User>>
            {
                new Mock<IUserValidator<User>>().Object
            };
            var mockPasswordValidator = new List<IPasswordValidator<User>>
            {
                new Mock<IPasswordValidator<User>>().Object
            };
            var mockLookupNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<User>>>();

            // Initialize mocks for each test
            _mockProjectMemberRepository = new Mock<IProjectMemberRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                mockOptions.Object,
                mockPasswordHasher.Object,
                mockUserValidator,
                mockPasswordValidator,
                mockLookupNormalizer.Object,
                mockErrors.Object,
                mockServiceProvider.Object,
                mockLogger.Object
            );
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new AddProjectMemberHandler(
                _mockProjectMemberRepository.Object,
                _mockProjectRepository.Object,
                _mockUserManager.Object,
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_AddsProjectMember_ReturnsProjectMemberDTO()
        {
            var command = new AddProjectMemberCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(command.ProjectId, command.UserId, command.Role);
            var projectMemberDTO = new ProjectMemberDTO
            {
                Id = projectMember.Id,
                ProjectId = projectMember.ProjectId,
                UserId = projectMember.UserId,
                Role = projectMember.Role,
            };

            _mockProjectMemberRepository.Setup(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId));
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.UserId.ToString())).ReturnsAsync(new User());
            _mockProjectMemberRepository.Setup(r => r.AddAsync(It.IsAny<ProjectMember>())).Callback<ProjectMember>(pm => projectMember = pm);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<ProjectMemberDTO>(It.IsAny<ProjectMember>())).Returns(projectMemberDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(projectMemberDTO.Id, result.Id);
            Assert.Equal(projectMemberDTO.ProjectId, result.ProjectId);
            Assert.Equal(projectMemberDTO.UserId, result.UserId);
            Assert.Equal(projectMemberDTO.Role, result.Role);
           
            _mockProjectMemberRepository.Verify(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId), Times.Once);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockUserManager.Verify(m => m.FindByIdAsync(command.UserId.ToString()), Times.Once);
            _mockProjectMemberRepository.Verify(r => r.AddAsync(projectMember), Times.Once);

            // Ensure the mapper's Map was called exactly once.
            _mockMapper.Verify(m => m.Map<ProjectMemberDTO>(It.IsAny<ProjectMember>()), Times.Once);
        }

        // Test exception throwing when a user is already in a project
        [Fact]
        public async Task VerifyUserAndProject_ThrowsException_WhenUserIsAlreadyInProject()
        {
            var command = new AddProjectMemberCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(command.ProjectId, command.UserId, command.Role);

            // Repository's Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId)).ReturnsAsync(projectMember);

            var exception = await Assert.ThrowsAsync<SprintManagerConflictException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"A user with ID {command.UserId} is already assigned to a project with ID {command.ProjectId}.", exception.Message);

            // Ensure GetByUserAndProjectIdAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId), Times.Once);
        }

        // Test exception throwing when a project is not found
        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectIsNotFound()
        {
            var command = new AddProjectMemberCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(command.ProjectId, command.UserId, command.Role);

            // Repository's Mock configuration
            _mockProjectMemberRepository.Setup(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId));
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {command.ProjectId} not found.", exception.Message);

            // Ensure GetByUserAndProjectIdAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
        }

        // Test exception throwing when a user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new AddProjectMemberCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Role = ProjectMemberRole.Developer
            };

            var projectMember = new ProjectMember(command.ProjectId, command.UserId, command.Role);

            _mockProjectMemberRepository.Setup(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId));
            _mockProjectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(new Project());
            _mockUserManager.Setup(m => m.FindByIdAsync(command.UserId.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {command.UserId} not found.", exception.Message);

            // Ensure GetByUserAndProjectIdAsync was called exactly once.
            _mockProjectMemberRepository.Verify(r => r.GetByUserAndProjectIdAsync(command.UserId, command.ProjectId), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockUserManager.Verify(m => m.FindByIdAsync(command.UserId.ToString()), Times.Once);
        }
    }
}