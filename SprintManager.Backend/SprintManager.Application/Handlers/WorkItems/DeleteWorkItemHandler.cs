using MediatR;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class DeleteWorkItemHandler : IRequestHandler<DeleteWorkItemCommand>
    {
        private readonly IWorkItemRepository _workItemRepository;

        public DeleteWorkItemHandler(IWorkItemRepository workItemRepository)
        {
            _workItemRepository = workItemRepository;
        }

        public async Task Handle(DeleteWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = await _workItemRepository.GetByIdAsync(request.Id);

            if (workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {request.Id} not found.");

            await _workItemRepository.DeleteAsync(workItem);
        }
    }
}