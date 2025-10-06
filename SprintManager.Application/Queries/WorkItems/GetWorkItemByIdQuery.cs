using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.WorkItems
{
    public class GetWorkItemByIdQuery : IRequest<WorkItemDto>
    {
        public Guid Id { get; set; }
    }
}