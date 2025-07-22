using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class UpdateProjectMemberRoleHandler : IRequestHandler<UpdateProjectMemberRoleCommand, ProjectMemberBasicDTO>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public UpdateProjectMemberRoleHandler(IProjectMemberRepository projectMemberRepository, IMapper mapper)
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task<ProjectMemberBasicDTO> Handle(UpdateProjectMemberRoleCommand request, CancellationToken cancellationToken)
        {
            var projectMember = await _projectMemberRepository.GetByUserAndProjectIdAsync(request.UserId, request.ProjectId);

            projectMember?.SetRole(request.Role);

            await _projectMemberRepository.UpdateAsync(projectMember);

            return _mapper.Map<ProjectMemberBasicDTO>(projectMember);
        }
    }
}