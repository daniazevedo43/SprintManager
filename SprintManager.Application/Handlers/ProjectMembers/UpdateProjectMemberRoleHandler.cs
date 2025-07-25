using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class UpdateProjectMemberRoleHandler : IRequestHandler<UpdateProjectMemberRoleCommand, ProjectMemberDTO>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public UpdateProjectMemberRoleHandler(IProjectMemberRepository projectMemberRepository, IMapper mapper)
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task<ProjectMemberDTO> Handle(UpdateProjectMemberRoleCommand request, CancellationToken cancellationToken)
        {
            var projectMember = await _projectMemberRepository.GetByIdAsync(request.Id);

            if (projectMember == null) throw new SprintManagerNotFoundException($"There's no relationship between a user and a project with ID {request.Id}.");

            projectMember?.SetRole(request.Role);

            await _projectMemberRepository.UpdateAsync(projectMember);

            return _mapper.Map<ProjectMemberDTO>(projectMember);
        }
    }
}