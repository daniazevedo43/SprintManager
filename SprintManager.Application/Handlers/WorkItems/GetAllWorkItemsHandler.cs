using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.WorkItems;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class GetAllWorkItemsHandler : IRequestHandler<GetAllWorkItemsQuery, List<WorkItemDto>>
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IMapper _mapper;

        public GetAllWorkItemsHandler(IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
        }

        public async Task<List<WorkItemDto>> Handle(GetAllWorkItemsQuery request, CancellationToken cancellationToken)
        {
            var workItems = await _workItemRepository.GetAllAsync();

            return _mapper.Map<List<WorkItemDto>>(workItems);
        }
    }
}