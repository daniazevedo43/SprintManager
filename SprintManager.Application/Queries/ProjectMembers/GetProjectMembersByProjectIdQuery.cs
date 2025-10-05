using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.ProjectMembers
{
    public class GetProjectMembersByProjectIdQuery : IRequest<List<ProjectMemberBasicDto>>
    {
        public Guid ProjectId { get; set; }
    }
}