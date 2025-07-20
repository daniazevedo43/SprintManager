using MediatR;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class RemoveProjectMemberHandler : IRequestHandler<RemoveProjectMemberCommand>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;

        public RemoveProjectMemberHandler(IProjectMemberRepository projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }

        public async Task Handle(RemoveProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectMemberRepository.GetByIdAsync(request.Id);

            if (project == null)
            {
                throw new SprintManagerNotFoundException($"There's no relationship between a user and a project with ID {request.Id}.");
            }

            await _projectMemberRepository.DeleteAsync(project);
        }
    }
}