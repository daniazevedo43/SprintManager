using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class SprintTests
    {
        // Test sprint creation without description
        [Fact]
        public void Sprint_Constructor_WithoutDescription_CreatesSprintSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            Assert.NotEqual(Guid.Empty, sprint.Id);
            Assert.Equal(projectId, sprint.ProjectId);
            Assert.Equal("Sprint 1", sprint.Name);
            Assert.Equal(new DateTime(2025, 7, 7).ToUniversalTime(), sprint.StartDate);
            Assert.Equal(new DateTime(2025, 7, 21).ToUniversalTime(), sprint.EndDate);
            Assert.Null(sprint.Description);
            Assert.Equal(Enums.SprintStatus.Planned, sprint.Status);
        }

        // Test sprint creation with description
        [Fact]
        public void Sprint_Constructor_WithDescription_CreatesSprintSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), "Description 1");

            Assert.NotEqual(Guid.Empty, sprint.Id);
            Assert.Equal(projectId, sprint.ProjectId);
            Assert.Equal("Sprint 1", sprint.Name);
            Assert.Equal(new DateTime(2025, 7, 7).ToUniversalTime(), sprint.StartDate);
            Assert.Equal(new DateTime(2025, 7, 21).ToUniversalTime(), sprint.EndDate);
            Assert.Equal("Description 1", sprint.Description);
            Assert.Equal(Enums.SprintStatus.Planned, sprint.Status);
        }

        // Test sprint's name change
        [Fact]
        public void SetName_UpdatesNameSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetName("Sprint 2");

            Assert.Equal("Sprint 2", sprint.Name);
        }

        // Test start date change
        [Fact]
        public void SetStartDate_UpdatesStartDateSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetStartDate(new DateTime(2025, 7, 8));

            Assert.Equal(new DateTime(2025, 7, 8).ToUniversalTime(), sprint.StartDate);
        }

        // Test end date change
        [Fact]
        public void SetEndDate_UpdatesEndDateSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetEndDate(new DateTime(2025, 7, 22));

            Assert.Equal(new DateTime(2025, 7, 22).ToUniversalTime(), sprint.EndDate);
        }

        // Test description change
        [Fact]
        public void SetDescription_UpdatesDescriptionSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), "Description 1");

            sprint.SetDescription("Description 2");

            Assert.Equal("Description 2", sprint.Description);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesStatusSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Sprint sprint = new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetStatus(SprintStatus.Active);

            Assert.Equal(SprintStatus.Active, sprint.Status);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Sprint(Guid.Empty, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
            );

            Assert.Equal("Project ID can't be null or empty. (Parameter 'projectId')", exception.Message);
        }

        // Test exception throwing when sprint's name is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty(string name)
        {
            Guid projectId = Guid.NewGuid();

            var exception = Assert.Throws<ArgumentException>(() =>
                new Sprint(projectId, name, new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
            );

            Assert.Equal("Sprint's name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when sprint's name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            Guid projectId = Guid.NewGuid();

            string name = new string('P', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Sprint(projectId, name, new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
            );

            Assert.Equal($"Sprint's name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when start date is higher than end date
        [Fact]
        public void VerifyStartDate_ThrowsException_WhenStartDateIsHigherThanEndDate()
        {
            Guid projectId = Guid.NewGuid();

            var exception = Assert.Throws<SprintManagerInvalidDateRangeException>(() =>
                new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 22), new DateTime(2025, 7, 21))
            );

            Assert.Equal($"Start date {new DateTime(2025, 7, 22)} is higher than end date {new DateTime(2025, 7, 21)}", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            Guid projectId = Guid.NewGuid();

            string description = new string('D', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Sprint(projectId, "Sprint 1", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), description)
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }
    }
}