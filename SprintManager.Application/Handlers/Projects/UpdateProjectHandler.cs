using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Projects;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Projects
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ProjectDTO>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public UpdateProjectHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            var existingProject = await _projectRepository.GetByNameAsync(request.Name);

            if (project == null)
            {
                throw new SprintManagerNotFoundException($"Project with ID {request?.Id} not found");
            }

            if (existingProject != null && project.Name != request.Name)
            {
                throw new SprintManagerConflictException($"A project called '{request.Name}' already exists.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                project.SetName(request.Name);
                project.SetStatus(project.Status);

                await _projectRepository.UpdateAsync(project);

                return _mapper.Map<ProjectDTO>(project);
            }
            else
            {
                project.SetName(request.Name);
                project.SetDescription(request.Description);
                project.SetStatus(project.Status);

                await _projectRepository.UpdateAsync(project);

                return _mapper.Map<ProjectDTO>(project);
            }
        }
    }
}