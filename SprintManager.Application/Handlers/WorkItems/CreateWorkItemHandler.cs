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
            if ((request.SprintId == Guid.Empty) && 
                (request.UserId == Guid.Empty) &&
                (string.IsNullOrWhiteSpace(request.Description)) &&
                (string.IsNullOrWhiteSpace(request.PriorityLevel.ToString())) &&
                (string.IsNullOrWhiteSpace(request.CompletionDate.ToString()) &&
                (string.IsNullOrWhiteSpace(request.HoursEstimate.ToString()))))
            {
                var workItem = new WorkItem(
                    request.ProjectId, 
                    request.WorkItemTitle,
                    request.WorkItemType
                );

                await _workItemRepository.AddAsync(workItem);
                return _mapper.Map<WorkItemDTO>(workItem);
            }
            else
            {
                var workItem = new WorkItem(
                    request.ProjectId, 
                    request.SprintId, 
                    request.UserId,
                    request.WorkItemTitle, 
                    request.WorkItemType,
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
} 