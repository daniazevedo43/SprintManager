using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class WorkItemDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid? SprintId { get; set; }
        public string? SprintName { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }
        public Guid? CreatorUserId { get; set; }
        public string WorkItemTitle { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public string? Description { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItemPriorityLevel? PriorityLevel { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? HoursEstimate { get; set; }
    }
}
