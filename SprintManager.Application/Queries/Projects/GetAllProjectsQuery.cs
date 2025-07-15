using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Projects
{
    public class GetAllProjectsQuery : IRequest<List<ProjectDTO>>
    {
    }
}
