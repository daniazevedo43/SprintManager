using SprintManager.Domain.Entities;

namespace SprintManager.Domain.Tests
{
    public class ProjectMemberTests
    {
        // Test project member creation
        [Fact]
        public void ProjectMember_Constructor_WithValidData_CreatesProjectSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            ProjectMember projectMember = new ProjectMember(project.Id, user.Id, Enums.ProjectMemberRole.Developer);

            Assert.NotEqual(Guid.Empty, projectMember.Id);
            Assert.Equal(project.Id, projectMember.ProjectId);
            Assert.Equal(user.Id, projectMember.UserId);
            Assert.Equal(Enums.ProjectMemberRole.Developer, projectMember.Role);
        }

        // Test project ID change
        [Fact]
        public void SetProjectId_UpdatesProjectIdSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");
            ProjectMember projectMember = new ProjectMember(project.Id, user.Id, Enums.ProjectMemberRole.Developer);

            Guid newProjectId = Guid.NewGuid();

            projectMember.SetProjectId(newProjectId);

            Assert.Equal(newProjectId, projectMember.ProjectId);
        }

        // Test user ID change
        [Fact]
        public void SetUserId_UpdatesUserIdSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");
            ProjectMember projectMember = new ProjectMember(project.Id, user.Id, Enums.ProjectMemberRole.Developer);

            Guid newUserId = Guid.NewGuid();

            projectMember.SetUserId(newUserId);

            Assert.Equal(newUserId, projectMember.UserId);
        }

        // Test role change
        [Fact]
        public void SetRole_UpdatesRoleSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");
            ProjectMember projectMember = new ProjectMember(project.Id, user.Id, Enums.ProjectMemberRole.Developer);

            projectMember.SetRole(Enums.ProjectMemberRole.Client);

            Assert.Equal(Enums.ProjectMemberRole.Client, projectMember.Role);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            var exception = Assert.Throws<ArgumentException>(() =>
                new ProjectMember(Guid.Empty, user.Id, Enums.ProjectMemberRole.Developer)
            );

            Assert.Equal("Project ID can't be null or empty. (Parameter 'projectId')", exception.Message);
        }

        // Test exception throwing when user ID is null or empty
        [Fact]
        public void VerifyUserId_ThrowsException_WhenUserIdIsNullOrEmpty()
        {
            Project project = new Project("Project 1", "Description 1");

            var exception = Assert.Throws<ArgumentException>(() =>
                new ProjectMember(project.Id, Guid.Empty, Enums.ProjectMemberRole.Developer)
            );

            Assert.Equal("User ID can't be null or empty. (Parameter 'userId')", exception.Message);
        }
    }
}
