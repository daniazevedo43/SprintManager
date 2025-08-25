using MediatR;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class DeleteSprintHandler : IRequestHandler<DeleteSprintCommand>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IWorkItemRepository _workItemRepository;

        public DeleteSprintHandler(ISprintRepository sprintRepository, IWorkItemRepository workItemRepository)
        {
            _sprintRepository = sprintRepository;
            _workItemRepository = workItemRepository;
        }

        public async Task Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.Id);
            var workItem = await _workItemRepository.GetBySprintIdAsync(request.Id);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.Id} not found.");
            if (workItem != null) throw new SprintManagerConflictException($"Sprint with ID {request.Id} has one or more work items.");

            await _sprintRepository.DeleteAsync(sprint);
        }
    }
}