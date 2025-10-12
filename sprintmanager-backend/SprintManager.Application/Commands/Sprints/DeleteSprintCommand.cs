using MediatR;

namespace SprintManager.Application.Commands.Sprints
{
    public class DeleteSprintCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}