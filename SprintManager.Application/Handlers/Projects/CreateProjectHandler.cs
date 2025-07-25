using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Projects
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDTO>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var existingProject = await _projectRepository.GetByNameAsync(request.Name);

            if (existingProject != null) throw new SprintManagerConflictException($"A project called '{request.Name}' already exists.");

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                var project = new Project(request.Name);
                await _projectRepository.AddAsync(project);
                return _mapper.Map<ProjectDTO>(project);
            } 
            else
            {
                var project = new Project(request.Name, request.Description);
                await _projectRepository.AddAsync(project);
                return _mapper.Map<ProjectDTO>(project);
            }
        }
    }
}