using MediatR;

namespace SprintManager.Application.Commands.ProjectMembers
{
    public class RemoveProjectMemberCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}