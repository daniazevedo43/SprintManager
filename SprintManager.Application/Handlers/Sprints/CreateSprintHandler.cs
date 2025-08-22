using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class CreateSprintHandler : IRequestHandler<CreateSprintCommand, SprintDTO>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public CreateSprintHandler(
            ISprintRepository sprintRepository, 
            IProjectRepository projectRepository,
            IMapper mapper
        )
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<SprintDTO> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
        {
            var existingSprint = await _sprintRepository.GetByProjectIdAndSprintNameAsync(request.ProjectId, request.SprintName);

            if (existingSprint != null) throw new SprintManagerConflictException($"A sprint called '{request.SprintName}' already exists in this project.");

            var project = await _projectRepository.GetByIdAsync(request.ProjectId); 

            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {request.ProjectId} not found.");

            var sprint = new Sprint(request.ProjectId, request.SprintName, request.StartDate, request.EndDate, request.Description);
            
            await _sprintRepository.AddAsync(sprint);
            
            return _mapper.Map<SprintDTO>(sprint);
        }
    }
}