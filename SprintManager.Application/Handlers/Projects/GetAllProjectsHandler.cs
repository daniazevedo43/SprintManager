using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Projects;

namespace SprintManager.Application.Handlers.Projects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetAllProjectsHandler(IProjectRepository projectRepository, IMapper mapper) 
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllAsync();

            return _mapper.Map<List<ProjectDto>>(projects);
        }
    }
}