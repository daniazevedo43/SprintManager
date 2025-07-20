using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.ProjectMembers;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class GetProjectMembersByProjectIdHandler : IRequestHandler<GetProjectMembersByProjectIdCommand, List<ProjectMemberBasicDTO>>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public GetProjectMembersByProjectIdHandler(IProjectMemberRepository projectMemberRepository, IMapper mapper)
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectMemberBasicDTO>> Handle(GetProjectMembersByProjectIdCommand request, CancellationToken cancellationToken)
        {
            var projectMembers = await _projectMemberRepository.GetMembersByProjectIdAsync(request.ProjectId);

            return _mapper.Map<List<ProjectMemberBasicDTO>>(projectMembers);
        }
    }
}
