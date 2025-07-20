using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.ProjectMembers
{
    public class GetProjectMembersByProjectIdCommand : IRequest<List<ProjectMemberBasicDTO>>
    {
        public Guid ProjectId { get; set; }
    }
}
