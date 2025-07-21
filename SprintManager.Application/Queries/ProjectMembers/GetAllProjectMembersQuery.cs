using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.ProjectMembers
{
    public class GetAllProjectMembersQuery : IRequest<List<ProjectMemberDTO>>
    {
    }
}