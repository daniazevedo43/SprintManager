using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Sprints
{
    public class CreateSprintHandler : IRequestHandler<CreateSprintCommand, SprintDTO>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public CreateSprintHandler(ISprintRepository sprintRepository, IMapper mapper)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        public async Task<SprintDTO> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
        {
            var existingSprint = await _sprintRepository.GetByProjectIdAndNameAsync(request.ProjectId, request.Name);

            if (existingSprint != null)
            {
                throw new SprintManagerConflictException($"A sprint called '{request.Name}' already exists in this project.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                var sprint = new Sprint(request.ProjectId, request.Name, request.StartDate, request.EndDate);
                await _sprintRepository.AddAsync(sprint);
                return _mapper.Map<SprintDTO>(sprint);
            }
            else
            {
                var sprint = new Sprint(request.ProjectId, request.Name, request.StartDate, request.EndDate, request.Description);
                await _sprintRepository.AddAsync(sprint);
                return _mapper.Map<SprintDTO>(sprint);
            }
        }
    }
}