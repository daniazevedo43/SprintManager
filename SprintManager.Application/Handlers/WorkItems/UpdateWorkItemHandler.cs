using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class UpdateWorkItemHandler : IRequestHandler<UpdateWorkItemCommand, WorkItemDto>
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateWorkItemHandler(
            IWorkItemRepository workItemRepository,
            ISprintRepository sprintRepository,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _workItemRepository = workItemRepository;
            _sprintRepository = sprintRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<WorkItemDto> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);

            User? user = null;

            if (request.AssignedUserId.HasValue)
                user = await _userManager.FindByIdAsync(request.AssignedUserId.ToString()!);

            if (!string.IsNullOrWhiteSpace(request.SprintId.ToString()) && sprint == null)
                throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

            if (!string.IsNullOrWhiteSpace(request.AssignedUserId.ToString()) && user == null)
                throw new SprintManagerNotFoundException($"User with ID {request.AssignedUserId} not found.");

            var workItem = await _workItemRepository.GetByIdAsync(request.Id);

            if (workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {request?.Id} not found.");

            workItem.SetSprintId(request.SprintId);
            workItem.SetAssignedUserId(request.AssignedUserId);
            workItem.SetWorkItemTitle(request.WorkItemTitle);
            workItem.SetWorkItemType(request.WorkItemType);
            workItem.SetDescription(request.Description);
            workItem.SetStatus(request.Status);
            workItem.SetPriorityLevel(request.PriorityLevel);
            workItem.SetCompletionDate(request.CompletionDate);
            workItem.SetHoursEstimate(request.HoursEstimate);

            await _workItemRepository.UpdateAsync(workItem);

            return _mapper.Map<WorkItemDto>(workItem);
        }
    }
}