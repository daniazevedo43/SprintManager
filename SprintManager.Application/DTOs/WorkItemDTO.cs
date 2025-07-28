using SprintManager.Domain.Enums;

namespace SprintManager.Application.DTOs
{
    public class WorkItemDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid? SprintId { get; set; }
        public string? SprintName { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string WorkItemTitle { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public string? Description { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItemPriorityLevel? PriorityLevel { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? HoursEstimate { get; set; }
    }
}