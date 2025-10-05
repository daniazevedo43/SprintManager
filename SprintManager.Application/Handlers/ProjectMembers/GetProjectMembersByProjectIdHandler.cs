using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.ProjectMembers;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class GetProjectMembersByProjectIdHandler : IRequestHandler<GetProjectMembersByProjectIdQuery, List<ProjectMemberBasicDto>>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectMembersByProjectIdHandler(
            IProjectMemberRepository projectMemberRepository, 
            IProjectRepository projectRepository,
            IMapper mapper)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectMemberBasicDto>> Handle(GetProjectMembersByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {request.ProjectId} not found.");

            var projectMembers = await _projectMemberRepository.GetMembersByProjectIdAsync(request.ProjectId);

            return _mapper.Map<List<ProjectMemberBasicDto>>(projectMembers);
        }
    }
}