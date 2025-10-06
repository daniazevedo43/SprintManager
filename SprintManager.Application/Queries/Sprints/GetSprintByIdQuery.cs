using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Sprints
{
    public class GetSprintByIdQuery : IRequest<SprintDto>
    {
        public Guid Id { get; set; }
    }
}