using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;

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
            Assert.Equal(DateTime.UtcNow.ToUniversalTime(), workItem.CreationDate, TimeSpan.FromSeconds(1));
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
            Assert.Equal(orkItemType.Task, workItem.WorkItemType);
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
            Guid projectId = Guid.NewGuid();
            Guid sprintId = Guid.NewGuid();
            Guid assignedUserId = Guid.NewGuid();
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                projectId, sprintId, assignedUserId, WorkItemType.Task,
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
            Guid projectId = Guid.NewGuid();
            Guid sprintId = Guid.NewGuid();
            Guid assignedUserId = Guid.NewGuid();
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                projectId, sprintId, assignedUserId, WorkItemType.Task,
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
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetWorkItemType(WorkItemType.Bug);

            Assert.Equal(WorkItemType.Bug, workItem.WorkItemType);
        }

        // Test work item title change
        [Fact]
        public void SetTitle_UpdatesSetTitleSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetTitle("Create a Sprint domain");

            Assert.Equal("Create a Sprint domain", workItem.Title);
        }


        // Test work item title change
        [Fact]
        public void SetDescription_UpdatesSetDescriptionSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            Guid sprintId = Guid.NewGuid();
            Guid assignedUserId = Guid.NewGuid();
            DateTime nextDate = DateTime.UtcNow.ToUniversalTime() + new TimeSpan(1, 0, 0, 0);

            WorkItem workItem = new WorkItem(
                projectId, sprintId, assignedUserId, WorkItemType.Task,
                "Create a WorkItem domain", "Description 1", nextDate, 5
            );

            workItem.SetDescription("Description 2");

            Assert.Equal("Description 2", workItem.Description);
        }

        // Test work item title change
        [Fact]
        public void SetStatus_UpdatesSetStatusSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetStatus(WorkItemStatus.Closed);

            Assert.Equal(WorkItemStatus.Closed, workItem.Status);
        }


        // Test work item title change
        [Fact]
        public void SetTitle_UpdatesSetTitleSuccessfully()
        {
            Guid projectId = Guid.NewGuid();
            WorkItem workItem = new WorkItem(projectId, WorkItemType.Task, "Create a WorkItem domain");

            workItem.SetTitle("Create a Sprint domain");

            Assert.Equal("Create a Sprint domain", workItem.Title);
        }
    }
}
