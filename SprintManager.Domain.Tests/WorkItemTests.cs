using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class WorkItemTests
    {
        // Test basic work item creation
        [Fact]
        public void WorkItem_Constructor_Basic_CreatesWorkItemSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Null(workItem.SprintId);
            Assert.Null(workItem.AssignedUserId);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Equal("Create a WorkItem domain", workItem.Title);
            Assert.Null(workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Equal(WorkItemPriorityLevel.NotSet, workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.ToUniversalTime().Date, workItem.CreationDate.Date);
            Assert.Null(workItem.CompletionDate);
            Assert.Null(workItem.TimeEstimate);
        }

        // Test work item creation with all parameters
        [Fact]
        public void WorkItem_Constructor_Full_CreatesWorkItemSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid sprintId = Guid.NewGuid();
            Guid assignedUserId = Guid.NewGuid();
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                projectId, sprintId, assignedUserId, WorkItemType.Task, 
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Equal(sprintId, workItem.SprintId);
            Assert.Equal(assignedUserId, workItem.AssignedUserId);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Equal("Create a WorkItem domain", workItem.Title);
            Assert.Equal("Description 1", workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Equal(WorkItemPriorityLevel.NotSet, workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.ToUniversalTime().Date, workItem.CreationDate.Date);
            Assert.Equal(nextDate.ToUniversalTime(), workItem.CompletionDate);
            Assert.Equal(5, workItem.TimeEstimate);
        }


        // Test project's ID change
        [Fact]
        public void SetProjectId_UpdatesProjectIdSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            Guid newProjectId = Guid.NewGuid();

            workItem.SetProjectId(newProjectId);

            Assert.Equal(newProjectId, workItem.ProjectId);
        }

        // Test sprint's ID change
        [Fact]
        public void SetSprintId_UpdatesSprintIdSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            Guid newSprintId = Guid.NewGuid();

            workItem.SetSprintId(newSprintId);

            Assert.Equal(newSprintId, workItem.SprintId);
        }

        // Test assigned user's ID change
        [Fact]
        public void SetAssignedUserId_UpdatesAssignedUserIdSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            Guid newAssignedUserId = Guid.NewGuid();

            workItem.SetAssignedUserId(newAssignedUserId);

            Assert.Equal(newAssignedUserId, workItem.AssignedUserId);
        }

        // Test work item type change
        [Fact]
        public void SetWorkItemType_UpdatesSetWorkItemTypeSuccessfully()
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetWorkItemType(WorkItemType.Bug);

            Assert.Equal(WorkItemType.Bug, workItem.WorkItemType);
        }

        // Test work item title change
        [Fact]
        public void SetTitle_UpdatesSetTitleSuccessfully()
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetTitle("Create a Sprint domain");

            Assert.Equal("Create a Sprint domain", workItem.Title);
        }


        // Test description change
        [Fact]
        public void SetDescription_UpdatesSetDescriptionSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            workItem.SetDescription("Description 2");

            Assert.Equal("Description 2", workItem.Description);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesSetStatusSuccessfully()
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetStatus(WorkItemStatus.Closed);

            Assert.Equal(WorkItemStatus.Closed, workItem.Status);
        }


        // Test priority level change
        [Fact]
        public void SetPriorityLevel_UpdatesSetPriorityLevelSuccessfully()
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetPriorityLevel(WorkItemPriorityLevel.Medium);

            Assert.Equal(WorkItemPriorityLevel.Medium, workItem.PriorityLevel);
        }

        // Test completion date change
        [Fact]
        public void SetCompletionDate_UpdatesSetCompletionDateSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            workItem.SetCompletionDate(nextDate + new TimeSpan(1, 0, 0, 0));

            Assert.Equal(nextDate + new TimeSpan(1, 0, 0, 0), workItem.CompletionDate);
        }

        // Test completion date change
        [Fact]
        public void SetTimeEstimate_UpdatesSetTimeEstimateSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            workItem.SetTimeEstimate(6);

            Assert.Equal(6, workItem.TimeEstimate);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new WorkItem(Guid.Empty, WorkItemType.Task, "Create a WorkItem domain")
            );

            Assert.Equal("Project ID can't be null or empty. (Parameter 'projectId')", exception.Message);
        }

        // Test exception throwing when title is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyTitle_ThrowsException_WhenTItleIsNullOrEmpty(string title)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new WorkItem(Guid.NewGuid(), WorkItemType.Task, title)
            );

            Assert.Equal("Work item's title can't be null or empty. (Parameter 'title')", exception.Message);
        }

        // Test exception throwing when title is too long
        [Fact]
        public void VerifyTitle_ThrowsException_WhenTitleIsTooLong()
        {
            string title = new string('C', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(Guid.NewGuid(), WorkItemType.Task, title)
            );

            Assert.Equal($"Work item's title is too long. (Max length '255') (Actual length '{title.Length}') (Parameter 'title')", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);
            
            string description = new string('D', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                    "Create a WorkItem domain", description, nextDate, 5)
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }

        // Test exception throwing when completion date is lower that current date
        [Fact]
        public void VerifyCompletionDate_ThrowsException_WhenCompletionDateIsLowerThanCurrentDate()
        {
            DateTime previousDate = DateTime.UtcNow.ToUniversalTime() - new TimeSpan(1, 0, 0, 0);

            var exception = Assert.Throws<SprintManagerDateNotAllowedException>(() =>
                new WorkItem(
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), WorkItemType.Task,
                    "Create a WorkItem domain", "Description 1", previousDate, 5)
            );

            Assert.Equal($"Completion date '{previousDate.ToString("dd/MM/yyyy")}' can't be lower than the current date ('{DateTime.UtcNow.ToUniversalTime().ToString("dd/MM/yyyy")}').", exception.Message);
        }
    }
}