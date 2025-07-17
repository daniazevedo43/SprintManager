using MediatR;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.Commands.Users;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);

            if (project == null)
            {
                throw new SprintManagerNotFoundException($"Project with ID {request.Id} not found.");
            }

            await _projectRepository.DeleteAsync(project);
        }
    }
}