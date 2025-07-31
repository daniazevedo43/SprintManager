using MediatR;

namespace SprintManager.Application.Commands.WorkItems
{
    public class DeleteWorkItemCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}