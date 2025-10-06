using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.ProjectMembers;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class GetAllProjectMembersHandler : IRequestHandler<GetAllProjectMembersQuery, List<ProjectMemberDto>>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public GetAllProjectMembersHandler(IProjectMemberRepository projectMemberRepository, IMapper mapper)
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectMemberDto>> Handle(GetAllProjectMembersQuery request, CancellationToken cancellationToken)
        {
            var projectMembers = await _projectMemberRepository.GetAllAsync();

            return _mapper.Map<List<ProjectMemberDto>>(projectMembers);
        }
    }
}