using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class CreateWorkItemHandler : IRequestHandler<CreateWorkItemCommand, WorkItemDTO>
    {
        public readonly IWorkItemRepository _workItemRepository;
        public readonly IMapper _mapper;

        public CreateWorkItemHandler(IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
        }

        public async Task<WorkItemDTO> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = new WorkItem(
                request.ProjectId, 
                request.WorkItemTitle, 
                request.WorkItemType,
                request.SprintId,
                request.UserId,
                request.Description,
                request.PriorityLevel,
                request.CompletionDate,
                request.HoursEstimate
            );
                
            await _workItemRepository.AddAsync(workItem);
            return _mapper.Map<WorkItemDTO>(workItem);
            
        }
    }
} 