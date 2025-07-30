using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class UpdateWorkItemHandler : IRequestHandler<UpdateWorkItemCommand, WorkItemDTO>
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IMapper _mapper;

        public UpdateWorkItemHandler(IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _workItemRepository = workItemRepository;
            _mapper = mapper;
        }

        public async Task<WorkItemDTO> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = await _workItemRepository.GetByIdAsync(request.Id);

            if (workItem == null) throw new SprintManagerNotFoundException($"Sprint with ID {request?.Id} not found.");

            workItem.SetSprintId(request.SprintId);
            workItem.SetAssignedUserId(request.UserId);
            workItem.SetWorkItemTitle(request.WorkItemTitle);
            workItem.SetWorkItemType(request.WorkItemType);
            workItem.SetDescription(request.Description);
            workItem.SetStatus(request.Status);
            workItem.SetPriorityLevel(request.PriorityLevel);
            workItem.SetCompletionDate(request.CompletionDate);
            workItem.SetHoursEstimate(request.HoursEstimate);

            await _workItemRepository.UpdateAsync(workItem);

            return _mapper.Map<WorkItemDTO>(workItem);
        }
    }
}