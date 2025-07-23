using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Sprints
{
    public class GetSprintByIdHandler : IRequestHandler<GetSprintByIdQuery, SprintDTO>
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public GetSprintByIdHandler(ISprintRepository sprintRepository, IMapper mapper)
        {
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        public async Task<SprintDTO> Handle(GetSprintByIdQuery request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.Id);

            if (sprint == null)
            {
                throw new SprintManagerNotFoundException($"Sprint with ID {request.Id} not found");
            }

            return _mapper.Map<SprintDTO>(sprint);
        }
    }
}