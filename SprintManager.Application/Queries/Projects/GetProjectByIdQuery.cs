using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Projects
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
    }
}