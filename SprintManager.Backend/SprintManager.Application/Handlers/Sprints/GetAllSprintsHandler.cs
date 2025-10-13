using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;

namespace SprintManager.Application.Handlers.Sprints
{
    public class GetAllSprintsHandler : IRequestHandler<GetAllSprintsQuery, List<SprintDto>>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public GetAllSprintsHandler(ISprintRepository sprintRepository, IMapper mapper)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        public async Task<List<SprintDto>> Handle(GetAllSprintsQuery request, CancellationToken cancellationToken)
        {
            var sprints = await _sprintRepository.GetAllAsync();

            return _mapper.Map<List<SprintDto>>(sprints);
        }
    }
}