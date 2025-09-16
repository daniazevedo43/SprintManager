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
            var sprint = new Sprint(projectId, "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            Assert.NotEqual(Guid.Empty, sprint.Id);
            Assert.Equal(projectId, sprint.ProjectId);
            Assert.Equal("Test sprint", sprint.SprintName);
            Assert.Equal(new DateTime(2025, 7, 7).ToUniversalTime(), sprint.StartDate);
            Assert.Equal(new DateTime(2025, 7, 21).ToUniversalTime(), sprint.EndDate);
            Assert.Null(sprint.Description);
            Assert.Equal(SprintStatus.Active, sprint.Status);
        }

        // Test sprint creation with description
        [Fact]
        public void Sprint_Constructor_WithDescription_CreatesSprintSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            var sprint = new Sprint(projectId, "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), "Test description");

            Assert.NotEqual(Guid.Empty, sprint.Id);
            Assert.Equal(projectId, sprint.ProjectId);
            Assert.Equal("Test sprint", sprint.SprintName);
            Assert.Equal(new DateTime(2025, 7, 7).ToUniversalTime(), sprint.StartDate);
            Assert.Equal(new DateTime(2025, 7, 21).ToUniversalTime(), sprint.EndDate);
            Assert.Equal("Test description", sprint.Description);
            Assert.Equal(SprintStatus.Active, sprint.Status);
        }

        // Test sprint's name change
        [Fact]
        public void SetSprintName_UpdatesNameSuccessfully()
        {
            var sprint = new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetSprintName("Test sprint 2");

            Assert.Equal("Test sprint 2", sprint.SprintName);
        }

        // Test start date and end date change
        [Fact]
        public void SetStartDate_UpdatesStartDateSuccessfully()
        {
            var sprint = new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetDates(new DateTime(2025, 7, 8), new DateTime(2025, 7, 22));

            Assert.Equal(new DateTime(2025, 7, 8).ToUniversalTime(), sprint.StartDate);
            Assert.Equal(new DateTime(2025, 7, 22).ToUniversalTime(), sprint.EndDate);
        }

        // Test description change
        [Fact]
        public void SetDescription_UpdatesDescriptionSuccessfully()
        {
            var sprint = new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), "Test description");

            sprint.SetDescription("Test description 2");

            Assert.Equal("Test description 2", sprint.Description);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesStatusSuccessfully()
        {
            var sprint = new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21));

            sprint.SetStatus(SprintStatus.Active);

            Assert.Equal(SprintStatus.Active, sprint.Status);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Sprint(Guid.Empty, "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
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
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Sprint(Guid.NewGuid(), name, new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
            );

            Assert.Equal("Sprint's name can't be null or empty. (Parameter 'sprintName')", exception.Message);
        }

        // Test exception throwing when sprint's name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('P', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Sprint(Guid.NewGuid(), name, new DateTime(2025, 7, 7), new DateTime(2025, 7, 21))
            );

            Assert.Equal($"Sprint's name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'sprintName')", exception.Message);
        }

        // Test exception throwing when start date is higher than end date
        [Fact]
        public void VerifyStartDate_ThrowsException_WhenStartDateIsHigherThanEndDate()
        {
            var exception = Assert.Throws<SprintManagerInvalidDateRangeException>(() =>
                new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 22), new DateTime(2025, 7, 21))
            );

            Assert.Equal($"Start date {new DateTime(2025, 7, 22).ToString("dd/MM/yyyy")} is higher than end date {new DateTime(2025, 7, 21).ToString("dd/MM/yyyy")}", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            string description = new string('D', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Sprint(Guid.NewGuid(), "Test sprint", new DateTime(2025, 7, 7), new DateTime(2025, 7, 21), description)
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }
    }
}