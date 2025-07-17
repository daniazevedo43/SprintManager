using MediatR;

namespace SprintManager.Application.Commands.Projects
{
    public class DeleteProjectCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}