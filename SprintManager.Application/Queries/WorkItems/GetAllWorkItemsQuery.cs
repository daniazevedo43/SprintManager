using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.WorkItems
{
    public class GetAllWorkItemsQuery : IRequest<List<WorkItemDTO>>
    {
    }
}