using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Projects
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var existingProject = await _projectRepository.GetByNameAsync(request.Name);

            if (existingProject != null) throw new SprintManagerConflictException($"A project called '{request.Name}' already exists.");

            var project = new Project(request.Name, request.Description);
            
            await _projectRepository.AddAsync(project);
            
            return _mapper.Map<ProjectDto>(project);
        }
    }
}