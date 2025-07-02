using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class ProjectTests
    {
        // Test project creation without description
        [Fact]
        public void Project_Constructor_WithoutDescription_CreatesProjectSuccessfully()
        {
            Project project = new Project("Project 1");

            Assert.NotEqual(Guid.Empty, project.Id);
            Assert.Equal("Project 1", project.Name); 
            Assert.Null(project.Description);
            Assert.Equal(Enums.ProjectStatus.Active, project.Status);
        }

        // Test project creation with description
        [Fact]
        public void Project_Constructor_WithDescription_CreatesProjectSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");

            Assert.NotEqual(Guid.Empty, project.Id);
            Assert.Equal("Project 1", project.Name);
            Assert.Equal("Description 1", project.Description);
            Assert.Equal(Enums.ProjectStatus.Active, project.Status);
        }

        // Test project's name change
        [Fact]
        public void SetName_UpdatesNameSuccessfully()
        {
            Project project = new Project("Project 1");

            project.SetName("Project 2");

            Assert.Equal("Project 2", project.Name);
        }

        // Test description change
        [Fact]
        public void SetDescription_UpdatesDescriptionSuccessfully()
        {
            Project project = new Project("Project 1", "Description 1");

            project.SetDescription("Description 2");

            Assert.Equal("Description 2", project.Description);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesStatusSuccessfully()
        {
            Project project = new Project("Project 1");

            project.SetStatus(Enums.ProjectStatus.Completed);

            Assert.Equal(Enums.ProjectStatus.Completed, project.Status);
        }

        // Test exception throwing when project's name is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Project(name)
            );

            Assert.Equal("Project's name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when project's name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('p', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Project(name)
            );

            Assert.Equal($"Project's name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            string description = new string('d', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Project("Project 1", description)
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }
    }
}
