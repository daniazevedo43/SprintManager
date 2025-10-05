using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Enums;

namespace SprintManager.Application.Commands.ProjectMembers
{
    public class AddProjectMemberCommand : IRequest<ProjectMemberDto>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectMemberRole Role { get; set; }
    }
}