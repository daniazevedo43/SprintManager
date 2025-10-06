using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.WorkItems
{
    public class UpdateWorkItemCommand : IRequest<WorkItemDto>
    {
        public Guid Id { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string WorkItemTitle { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public string? Description { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItemPriorityLevel? PriorityLevel { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? HoursEstimate { get; set; }
    }
}