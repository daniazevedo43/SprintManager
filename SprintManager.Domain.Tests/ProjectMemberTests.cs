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
    }
}
