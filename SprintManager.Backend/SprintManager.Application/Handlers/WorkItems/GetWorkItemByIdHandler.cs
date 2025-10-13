using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.WorkItems;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class GetWorkItemByIdHandler : IRequestHandler<GetWorkItemByIdQuery, WorkItemDto>
    {
        public readonly IWorkItemRepository _workItemRepository;
        public readonly IMapper _mapper;

        public GetWorkItemByIdHandler(IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
        }

        public async Task<WorkItemDto> Handle(GetWorkItemByIdQuery request, CancellationToken cancellationToken)
        {
            var workItem = await _workItemRepository.GetByIdAsync(request.Id);

            if (workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {request.Id} not found.");

            return _mapper.Map<WorkItemDto>(workItem);
        }
    }
}