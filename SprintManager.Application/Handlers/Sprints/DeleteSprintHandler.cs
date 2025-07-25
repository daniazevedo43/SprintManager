using MediatR;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class DeleteSprintHandler : IRequestHandler<DeleteSprintCommand>
    {
        private readonly ISprintRepository _sprintRepository;

        public DeleteSprintHandler(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.Id);

            if (sprint == null) throw new SprintManagerNotFoundException($"Sprint with ID {request.Id} not found.");

            await _sprintRepository.DeleteAsync(sprint);
        }
    }
}