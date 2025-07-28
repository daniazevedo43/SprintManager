using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class WorkItem
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid? SprintId { get; private set; }
        public Guid? UserId { get; private set; }
        public WorkItemType WorkItemType { get; private set; }
        public string WorkItemTitle { get; private set; }
        public string? Description { get; private set; }
        public WorkItemStatus Status { get; private set; }
        public WorkItemPriorityLevel? PriorityLevel { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? CompletionDate { get; private set; }
        public int? HoursEstimate { get; private set; }
        public Project? Project { get; private set; }
        public Sprint? Sprint { get; private set; }
        public User? User { get; private set; }

        public WorkItem()
        {

        }

        public WorkItem(Guid projectId, WorkItemType workItemType, string workItemTitle)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            if(string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentException("Work item's title can't be null or empty.", nameof(workItemTitle));
            if(workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            WorkItemType = workItemType;
            WorkItemTitle = workItemTitle;
            Status = WorkItemStatus.New;
            PriorityLevel = WorkItemPriorityLevel.NotSet;
            CreationDate = DateTime.UtcNow;
        }

        public WorkItem(Guid projectId, Guid sprintId, Guid userId, WorkItemType workItemType, string workItemTitle, string description, DateTime completionDate, int hoursEstimate)
        {
            if(projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            if(string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentException("Work item's title can't be null or empty.", nameof(workItemTitle));
            if(workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));
            if(description.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));
            if(completionDate < DateTime.UtcNow) throw new SprintManagerDateNotAllowedException($"Completion date '{completionDate.ToString("dd/MM/yyyy")}' can't be lower than the current date ('{DateTime.UtcNow.ToString("dd/MM/yyyy")}').", nameof(completionDate));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            SprintId = sprintId;
            UserId = userId;
            WorkItemType = workItemType;
            WorkItemTitle = workItemTitle;
            Description = description;
            Status = WorkItemStatus.New;
            PriorityLevel = WorkItemPriorityLevel.NotSet;
            CreationDate = DateTime.UtcNow;
            CompletionDate = completionDate.ToUniversalTime();
            HoursEstimate = hoursEstimate;
        }

        // Update work item's projectId
        public void SetProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty) throw new ArgumentException("Project ID can't be null or empty.", nameof(projectId));
            ProjectId = projectId;
        }

        // Update work item's sprint
        public void SetSprintId(Guid sprintId)
        {
            SprintId = sprintId;
        }

        // Update work item's assigned user
        public void SetAssignedUserId(Guid userId)
        {
            UserId = userId;
        }

        // Update work item's type
        public void SetWorkItemType(WorkItemType workItemType)
        {
            WorkItemType = workItemType;
        }

        // Update work item's title
        public void SetWorkItemTitle(string workItemTitle)
        {
            if(string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentException("Work item's title can't be null or empty.", nameof(workItemTitle));
            if(workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));
            WorkItemTitle = workItemTitle;
        }

        // Update work item's description
        public void SetDescription(string description)
        {
            if (description.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));
            Description = description;
        }

        // Update work item's status
        public void SetStatus(WorkItemStatus status)
        {
            Status = status;
        }

        // Update work item's priority level
        public void SetPriorityLevel(WorkItemPriorityLevel priorityLevel)
        {
            PriorityLevel = priorityLevel;
        }

        // Update work item's completion date
        public void SetCompletionDate(DateTime completionDate)
        {
            if(completionDate < DateTime.UtcNow.ToUniversalTime()) throw new SprintManagerDateNotAllowedException($"Completion date '{completionDate.ToString("dd/MM/yyyy")}' can't be lower than the current date ('{DateTime.UtcNow.ToUniversalTime().ToString("dd/MM/yyyy")}').", nameof(completionDate));
            CompletionDate = completionDate.ToUniversalTime();
        }

        // Update work item's time estimate
        public void SetHoursEstimate(int hoursEstimate)
        {
            HoursEstimate = hoursEstimate;
        }
    }
}