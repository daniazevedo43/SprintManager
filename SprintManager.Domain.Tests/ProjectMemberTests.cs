using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

namespace SprintManager.Domain.Tests
{
    public class ProjectMemberTests
    {
        // Test project member creation
        [Fact]
        public void ProjectMember_Constructor_WithValidData_CreatesProjectSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ProjectMember projectMember = new ProjectMember(projectId, userId, ProjectMemberRole.Developer);

            Assert.NotEqual(Guid.Empty, projectMember.Id);
            Assert.Equal(projectId, projectMember.ProjectId);
            Assert.Equal(userId, projectMember.UserId);
            Assert.Equal(ProjectMemberRole.Developer, projectMember.Role);
        }

        // Test project ID change
        [Fact]
        public void SetProjectId_UpdatesProjectIdSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ProjectMember projectMember = new ProjectMember(projectId, userId, ProjectMemberRole.Developer);

            Guid newProjectId = Guid.NewGuid();

            projectMember.SetProjectId(newProjectId);

            Assert.Equal(newProjectId, projectMember.ProjectId);
        }

        // Test user ID change
        [Fact]
        public void SetUserId_UpdatesUserIdSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ProjectMember projectMember = new ProjectMember(projectId, userId, ProjectMemberRole.Developer);

            Guid newUserId = Guid.NewGuid();

            projectMember.SetUserId(newUserId);

            Assert.Equal(newUserId, projectMember.UserId);
        }

        // Test role change
        [Fact]
        public void SetRole_UpdatesRoleSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            ProjectMember projectMember = new ProjectMember(projectId, userId, ProjectMemberRole.Developer);

            projectMember.SetRole(ProjectMemberRole.Client);

            Assert.Equal(ProjectMemberRole.Client, projectMember.Role);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            Guid userId = Guid.NewGuid();

            var exception = Assert.Throws<ArgumentException>(() =>
                new ProjectMember(Guid.Empty, userId, ProjectMemberRole.Developer)
            );

            Assert.Equal("Project ID can't be null or empty. (Parameter 'projectId')", exception.Message);
        }

        // Test exception throwing when user ID is null or empty
        [Fact]
        public void VerifyUserId_ThrowsException_WhenUserIdIsNullOrEmpty()
        {
            Guid projectId = Guid.NewGuid();

            var exception = Assert.Throws<ArgumentException>(() =>
                new ProjectMember(projectId, Guid.Empty, ProjectMemberRole.Developer)
            );

            Assert.Equal("User ID can't be null or empty. (Parameter 'userId')", exception.Message);
        }
    }
}
