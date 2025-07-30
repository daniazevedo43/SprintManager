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
            var workItem = new WorkItem(projectId, "Create a WorkItem domain", WorkItemType.Task);

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Null(workItem.SprintId);
            Assert.Null(workItem.UserId);
            Assert.Equal("Create a WorkItem domain", workItem.WorkItemTitle);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Null(workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Null(workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.Date, workItem.CreationDate.Date);
            Assert.Null(workItem.CompletionDate);
            Assert.Null(workItem.HoursEstimate);
        }

        // Test work item creation with all parameters
        [Fact]
        public void WorkItem_Constructor_Full_CreatesWorkItemSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid sprintId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                projectId, "Create a WorkItem domain",
                WorkItemType.Task, sprintId, userId, "Description 1", 
                WorkItemPriorityLevel.Low, nextDate, 5
            );

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Equal(sprintId, workItem.SprintId);
            Assert.Equal(userId, workItem.UserId);
            Assert.Equal("Create a WorkItem domain", workItem.WorkItemTitle);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Equal("Description 1", workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Equal(WorkItemPriorityLevel.Low, workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.Date, workItem.CreationDate.Date);
            Assert.Equal(nextDate.ToUniversalTime(), workItem.CompletionDate);
            Assert.Equal(5, workItem.HoursEstimate);
        }

        // Test sprint's ID change
        [Fact]
        public void SetSprintId_UpdatesSprintIdSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Create a WorkItem domain",
                WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                "Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            Guid newSprintId = Guid.NewGuid();

            workItem.SetSprintId(newSprintId);

            Assert.Equal(newSprintId, workItem.SprintId);
        }

        // Test assigned user's ID change
        [Fact]
        public void SetAssignedUserId_UpdatesAssignedUserIdSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Create a WorkItem domain", 
                WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                "Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            Guid userId = Guid.NewGuid();

            workItem.SetAssignedUserId(userId);

            Assert.Equal(userId, workItem.UserId);
        }

        // Test work item's title change
        [Fact]
        public void SetWorkItemTitle_UpdatesWorkItemTitleSuccessfully()
        {
            var workItem = new WorkItem(Guid.NewGuid(), "Create a WorkItem domain", WorkItemType.Task);

            workItem.SetWorkItemTitle("Create a Sprint domain");

            Assert.Equal("Create a Sprint domain", workItem.WorkItemTitle);
        }

        // Test work item's type change
        [Fact]
        public void SetWorkItemType_UpdatesWorkItemTypeSuccessfully()
        {
            var workItem = new WorkItem(Guid.NewGuid(), "Create a WorkItem domain", WorkItemType.Task);

            workItem.SetWorkItemType(WorkItemType.Bug);

            Assert.Equal(WorkItemType.Bug, workItem.WorkItemType);
        }

        // Test description change
        [Fact]
        public void SetDescription_UpdatesDescriptionSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Create a WorkItem domain", 
                WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                "Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetDescription("Description 2");

            Assert.Equal("Description 2", workItem.Description);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesStatusSuccessfully()
        {
            var workItem = new WorkItem(Guid.NewGuid(), "Create a WorkItem domain", WorkItemType.Task);

            workItem.SetStatus(WorkItemStatus.Closed);

            Assert.Equal(WorkItemStatus.Closed, workItem.Status);
        }

        // Test priority level change
        [Fact]
        public void SetPriorityLevel_UpdatesPriorityLevelSuccessfully()
        {
            var workItem = new WorkItem(Guid.NewGuid(), "Create a WorkItem domain", WorkItemType.Task);

            workItem.SetPriorityLevel(WorkItemPriorityLevel.Medium);

            Assert.Equal(WorkItemPriorityLevel.Medium, workItem.PriorityLevel);
        }

        // Test completion date change
        [Fact]
        public void SetCompletionDate_UpdatesCompletionDateSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Create a WorkItem domain",
                WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(),
                "Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetCompletionDate(nextDate + new TimeSpan(1, 0, 0, 0));

            Assert.Equal(nextDate + new TimeSpan(1, 0, 0, 0), workItem.CompletionDate);
        }

        // Test completion date change
        [Fact]
        public void SetHoursEstimate_UpdatesHoursEstimateSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Create a WorkItem domain",
                WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                "Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetHoursEstimate(6);

            Assert.Equal(6, workItem.HoursEstimate);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new WorkItem(Guid.Empty, "Create a WorkItem domain", WorkItemType.Task)
            );

            Assert.Equal("Project ID can't be null or empty. (Parameter 'projectId')", exception.Message);
        }

        // Test exception throwing when title is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyWorkItemTitle_ThrowsException_WhenWorkItemTitleIsNullOrEmpty(string title)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new WorkItem(Guid.NewGuid(), title, WorkItemType.Task)
            );

            Assert.Equal("Work item's title can't be null or empty. (Parameter 'workItemTitle')", exception.Message);
        }

        // Test exception throwing when title is too long
        [Fact]
        public void VerifyWorkItemTitle_ThrowsException_WhenWorkItemTitleIsTooLong()
        {
            string title = new string('C', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(Guid.NewGuid(), title, WorkItemType.Task)
            );

            Assert.Equal($"Work item's title is too long. (Max length '255') (Actual length '{title.Length}') (Parameter 'workItemTitle')", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);
            
            string description = new string('D', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(
                    Guid.NewGuid(), "Create a WorkItem domain", 
                    WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                    description, WorkItemPriorityLevel.Low, nextDate, 5
                )
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }

        // Test exception throwing when completion date is lower that current date
        [Fact]
        public void VerifyCompletionDate_ThrowsException_WhenCompletionDateIsLowerThanCurrentDate()
        {
            DateTime previousDate = DateTime.UtcNow - new TimeSpan(1, 0, 0, 0);

            var exception = Assert.Throws<SprintManagerDateNotAllowedException>(() =>
                new WorkItem(
                    Guid.NewGuid(), "Create a WorkItem domain", 
                    WorkItemType.Task, Guid.NewGuid(), Guid.NewGuid(), 
                    "Description 1", WorkItemPriorityLevel.Low, previousDate, 
                    5
                )
            );

            Assert.Equal($"Completion date '{previousDate.ToString("dd/MM/yyyy")}' can't be lower than the current date ('{DateTime.UtcNow.ToString("dd/MM/yyyy")}').", exception.Message);
        }
    }
}