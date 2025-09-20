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
            var projectId = Guid.NewGuid();
            var creatorUserId = Guid.NewGuid();

            var workItem = new WorkItem(
                projectId, "Test title", WorkItemType.Task, creatorUserId
            );

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Null(workItem.SprintId);
            Assert.Null(workItem.AssignedUserId);
            Assert.Equal(creatorUserId, workItem.CreatorUserId);
            Assert.Equal("Test title", workItem.WorkItemTitle);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Null(workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Null(workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.Date, workItem.CreationDate.Date);
            Assert.Null(workItem.UpdateDate?.Date);
            Assert.Null(workItem.CompletionDate);
            Assert.Null(workItem.HoursEstimate);
        }

        // Test work item creation with all parameters
        [Fact]
        public void WorkItem_Constructor_Full_CreatesWorkItemSuccessfully()
        {
            var projectId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var assignedUserId = Guid.NewGuid();
            var creatorUserId = Guid.NewGuid();
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                projectId, "Test title", WorkItemType.Task, sprintId,
                assignedUserId, creatorUserId, "Test Description", 
                WorkItemPriorityLevel.Low, nextDate, 5
            );

            Assert.NotEqual(Guid.Empty, workItem.Id);
            Assert.Equal(projectId, workItem.ProjectId);
            Assert.Equal(sprintId, workItem.SprintId);
            Assert.Equal(assignedUserId, workItem.AssignedUserId);
            Assert.Equal(creatorUserId, workItem.CreatorUserId);
            Assert.Equal("Test title", workItem.WorkItemTitle);
            Assert.Equal(WorkItemType.Task, workItem.WorkItemType);
            Assert.Equal("Test Description", workItem.Description);
            Assert.Equal(WorkItemStatus.New, workItem.Status);
            Assert.Equal(WorkItemPriorityLevel.Low, workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.Date, workItem.CreationDate.Date);
            Assert.Null(workItem.UpdateDate?.Date);
            Assert.Equal(nextDate.ToUniversalTime(), workItem.CompletionDate);
            Assert.Equal(5, workItem.HoursEstimate);
        }

        // Test sprint's ID change
        [Fact]
        public void SetSprintId_UpdatesSprintIdSuccessfully()
        {
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Test Description", WorkItemPriorityLevel.Low, nextDate, 5
            );

            var newSprintId = Guid.NewGuid();

            workItem.SetSprintId(newSprintId);

            Assert.Equal(newSprintId, workItem.SprintId);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test assigned user's ID change
        [Fact]
        public void SetAssignedUserId_UpdatesAssignedUserIdSuccessfully()
        {
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Test Description", WorkItemPriorityLevel.Low, nextDate, 5
            );

            var assignedUserId = Guid.NewGuid();

            workItem.SetAssignedUserId(assignedUserId);

            Assert.Equal(assignedUserId, workItem.AssignedUserId);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test work item's title change
        [Fact]
        public void SetWorkItemTitle_UpdatesWorkItemTitleSuccessfully()
        {
            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid()
            );

            workItem.SetWorkItemTitle("Test title 2");

            Assert.Equal("Test title 2", workItem.WorkItemTitle);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test work item's type change
        [Fact]
        public void SetWorkItemType_UpdatesWorkItemTypeSuccessfully()
        {
            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid()
            );

            workItem.SetWorkItemType(WorkItemType.Bug);

            Assert.Equal(WorkItemType.Bug, workItem.WorkItemType);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test description change
        [Fact]
        public void SetDescription_UpdatesDescriptionSuccessfully()
        {
            DateTime nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Test Description 1", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetDescription("Test Description 2");

            Assert.Equal("Test Description 2", workItem.Description);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test status change
        [Fact]
        public void SetStatus_UpdatesStatusSuccessfully()
        {
            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid()
            );

            workItem.SetStatus(WorkItemStatus.Closed);

            Assert.Equal(WorkItemStatus.Closed, workItem.Status);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test priority level change
        [Fact]
        public void SetPriorityLevel_UpdatesPriorityLevelSuccessfully()
        {
            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid()
            );

            workItem.SetPriorityLevel(WorkItemPriorityLevel.Medium);

            Assert.Equal(WorkItemPriorityLevel.Medium, workItem.PriorityLevel);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test completion date change
        [Fact]
        public void SetCompletionDate_UpdatesCompletionDateSuccessfully()
        {
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Test Description", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetCompletionDate(nextDate + new TimeSpan(1, 0, 0, 0));

            Assert.Equal(nextDate + new TimeSpan(1, 0, 0, 0), workItem.CompletionDate);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test completion date change
        [Fact]
        public void SetHoursEstimate_UpdatesHoursEstimateSuccessfully()
        {
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var workItem = new WorkItem(
                Guid.NewGuid(), "Test title", WorkItemType.Task, 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                "Test Description", WorkItemPriorityLevel.Low, nextDate, 5
            );

            workItem.SetHoursEstimate(6);

            Assert.Equal(6, workItem.HoursEstimate);
            Assert.Equal(DateTime.UtcNow.Date, workItem.UpdateDate?.Date);
        }

        // Test exception throwing when project ID is null or empty
        [Fact]
        public void VerifyProjectId_ThrowsException_WhenProjectIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new WorkItem(Guid.Empty, "Test title", WorkItemType.Task, Guid.NewGuid())
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
                new WorkItem(Guid.NewGuid(), title, WorkItemType.Task, Guid.NewGuid())
            );

            Assert.Equal("Work item's title can't be null or empty. (Parameter 'workItemTitle')", exception.Message);
        }

        // Test exception throwing when title is too long
        [Fact]
        public void VerifyWorkItemTitle_ThrowsException_WhenWorkItemTitleIsTooLong()
        {
            var title = new string('T', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(Guid.NewGuid(), title, WorkItemType.Task, Guid.NewGuid())
            );

            Assert.Equal($"Work item's title is too long. (Max length '255') (Actual length '{title.Length}') (Parameter 'workItemTitle')", exception.Message);
        }

        // Test exception throwing when description is too long
        [Fact]
        public void VerifyDescription_ThrowsException_WhenDescriptionIsTooLong()
        {
            var nextDate = DateTime.UtcNow + new TimeSpan(1, 0, 0, 0);

            var description = new string('T', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new WorkItem(
                    Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid(), 
                    Guid.NewGuid(), Guid.NewGuid(), description, 
                    WorkItemPriorityLevel.Low, nextDate, 5
                )
            );

            Assert.Equal($"Description is too long. (Max length '500') (Actual length '{description.Length}') (Parameter 'description')", exception.Message);
        }

        // Test exception throwing when completion date is lower that current date
        [Fact]
        public void VerifyCompletionDate_ThrowsException_WhenCompletionDateIsLowerThanCreationDate()
        {
            var previousDate = DateTime.UtcNow - new TimeSpan(1, 0, 0, 0);

            var exception = Assert.Throws<SprintManagerDateNotAllowedException>(() =>
                new WorkItem(
                    Guid.NewGuid(), "Test title", WorkItemType.Task, Guid.NewGuid(), 
                    Guid.NewGuid(), Guid.NewGuid(), "Test Description", 
                    WorkItemPriorityLevel.Low, previousDate, 5
                )
            );

            Assert.Equal($"Completion date '{previousDate.ToString("dd/MM/yyyy")}' can't be lower than the work item's creation date ('{DateTime.UtcNow.ToString("dd/MM/yyyy")}').", exception.Message);
        }
    }
}