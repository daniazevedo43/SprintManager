using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.ProjectMembers
{
    public class UpdateProjectMemberRoleCommand : IRequest<ProjectMemberDto>
    {
        public Guid Id { get; set; }
        public ProjectMemberRole Role { get; set; }
    }
}