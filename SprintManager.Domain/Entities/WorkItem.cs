using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class WorkItem
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid? SprintId { get; private set; }
        public Guid? AssignedUserId { get; private set; }
        public Guid CreatorUserId { get; private set; }
        public string WorkItemTitle { get; private set; }
        public WorkItemType WorkItemType { get; private set; }
        public string? Description { get; private set; }
        public WorkItemStatus Status { get; private set; }
        public WorkItemPriorityLevel? PriorityLevel { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public DateTime? CompletionDate { get; private set; }
        public int? HoursEstimate { get; private set; }
        public Project? Project { get; private set; }
        public Sprint? Sprint { get; private set; }
        public User? User { get; private set; }

        public WorkItem()
        {
        }

        public WorkItem(Guid projectId, Guid creatorUserId, string workItemTitle, WorkItemType workItemType)
        {
            if (projectId == Guid.Empty) throw new ArgumentNullException(nameof(projectId), "Project ID can't be null or empty.");
            if (creatorUserId == Guid.Empty) throw new ArgumentNullException(nameof(creatorUserId), "Creator user ID can't be null or empty.");
            if (string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentNullException(nameof(workItemTitle), "Work item's title can't be null or empty.");
            if (workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            CreatorUserId = creatorUserId;
            WorkItemTitle = workItemTitle;
            WorkItemType = workItemType;
            Status = WorkItemStatus.New;
            CreationDate = DateTime.UtcNow;
        }

        public WorkItem(Guid projectId, Guid creatorUserId, string workItemTitle, WorkItemType workItemType, Guid? sprintId, Guid? assignedUserId, string? description, WorkItemPriorityLevel? priorityLevel, DateTime? completionDate, int? hoursEstimate)
        {
            if (projectId == Guid.Empty) throw new ArgumentNullException(nameof(projectId), "Project ID can't be null or empty.");
            if (creatorUserId == Guid.Empty) throw new ArgumentNullException(nameof(creatorUserId), "Creator user ID can't be null or empty.");
            if (string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentNullException(nameof(workItemTitle), "Work item's title can't be null or empty.");
            if (workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));
            if (description?.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));

            Id = Guid.NewGuid();
            ProjectId = projectId;
            SprintId = sprintId;
            AssignedUserId = assignedUserId;
            CreatorUserId = creatorUserId;
            WorkItemTitle = workItemTitle;
            WorkItemType = workItemType;
            Description = description;
            Status = WorkItemStatus.New;
            PriorityLevel = priorityLevel;
            CreationDate = DateTime.UtcNow;

            if (completionDate < CreationDate) throw new SprintManagerDateNotAllowedException($"Completion date '{completionDate?.ToString("dd/MM/yyyy")}' can't be lower than the work item's creation date ('{CreationDate.ToString("dd/MM/yyyy")}').", nameof(completionDate));

            CompletionDate = completionDate?.ToUniversalTime();
            HoursEstimate = hoursEstimate;
        }

        // Update work item's sprint
        public void SetSprintId(Guid? sprintId)
        {
            SprintId = sprintId;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's assigned user
        public void SetAssignedUserId(Guid? assignedUserId)
        {
            AssignedUserId = assignedUserId;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's title
        public void SetWorkItemTitle(string workItemTitle)
        {
            if (string.IsNullOrWhiteSpace(workItemTitle)) throw new ArgumentNullException(nameof(workItemTitle), "Work item's title can't be null or empty.");
            if (workItemTitle.Length > 255) throw new SprintManagerTooLongException("Work item's title is too long.", 255, workItemTitle.Length, nameof(workItemTitle));
            
            WorkItemTitle = workItemTitle;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's type
        public void SetWorkItemType(WorkItemType workItemType)
        {
            WorkItemType = workItemType;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's description
        public void SetDescription(string? description)
        {
            if (description?.Length > 500) throw new SprintManagerTooLongException("Description is too long.", 500, description.Length, nameof(description));
            
            Description = description;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's status
        public void SetStatus(WorkItemStatus status)
        {
            Status = status;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's priority level
        public void SetPriorityLevel(WorkItemPriorityLevel? priorityLevel)
        {
            PriorityLevel = priorityLevel;
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's completion date
        public void SetCompletionDate(DateTime? completionDate)
        {
            if (completionDate < CreationDate) throw new SprintManagerDateNotAllowedException($"Completion date '{completionDate?.ToString("dd/MM/yyyy")}' can't be lower than the work item's creation date ('{CreationDate.ToString("dd/MM/yyyy")}').", nameof(completionDate));
            
            CompletionDate = completionDate?.ToUniversalTime();
            UpdateDate = DateTime.UtcNow;
        }

        // Update work item's time estimate
        public void SetHoursEstimate(int? hoursEstimate)
        {
            HoursEstimate = hoursEstimate;
            UpdateDate = DateTime.UtcNow;
        }
    }
}