using AutoMapper;
using SprintManager.Application.Commands.Sprints;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class UpdateSprintHandler
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public UpdateSprintHandler(ISprintRepository sprintRepository, IMapper mapper)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        public async Task<SprintDTO> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.Id);

            if (sprint == null)
            {
                throw new SprintManagerNotFoundException($"Sprint with ID {request?.Id} not found.");
            }

            sprint.SetName(request.Name);
            sprint.SetStartDate(request.StartDate);
            sprint.SetEndDate(request.EndDate);
            sprint.SetDescription(request.Description);
            sprint.SetStatus(request.Status);

            await _sprintRepository.UpdateAsync(sprint);

            return _mapper.Map<SprintDTO>(sprint);
        }
    }
}