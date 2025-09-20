using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.WorkItems
{
    public class CreateWorkItemCommand : IRequest<WorkItemDTO>
    {
        public Guid ProjectId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string WorkItemTitle { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public string? Description { get; set; }
        public WorkItemPriorityLevel? PriorityLevel { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? HoursEstimate { get; set; }
    }
}