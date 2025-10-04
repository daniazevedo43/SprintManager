using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.WorkItems
{
    public class CreateWorkItemHandler : IRequestHandler<CreateWorkItemCommand, WorkItemDTO>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateWorkItemHandler(
            IHttpContextAccessor httpContextAccessor,
            IWorkItemRepository workItemRepository,
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _workItemRepository = workItemRepository;
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<WorkItemDTO> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var creatorUserIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(creatorUserIdClaim) || !Guid.TryParse(creatorUserIdClaim, out var creatorUserId))
                throw new UnauthorizedAccessException("User not authenticated.");

            var projectId = await _projectRepository.GetByIdAsync(request.ProjectId);
            var sprintId = await _sprintRepository.GetByIdAsync(request.SprintId);
            
            User? assignedUserId = null;

            if(request.AssignedUserId.HasValue)
                assignedUserId = await _userManager.FindByIdAsync(request.AssignedUserId.Value.ToString());

            if (!string.IsNullOrWhiteSpace(request.ProjectId.ToString()) && projectId == null)
                throw new SprintManagerNotFoundException($"Project with ID {request.ProjectId} not found.");

            if (!string.IsNullOrWhiteSpace(request.SprintId.ToString()) && sprintId == null)
                throw new SprintManagerNotFoundException($"Sprint with ID {request.SprintId} not found.");

            if (!string.IsNullOrWhiteSpace(request.AssignedUserId.ToString()) && assignedUserId == null)
                throw new SprintManagerNotFoundException($"User with ID {request.AssignedUserId} not found.");

            var workItem = new WorkItem(
                request.ProjectId,
                request.WorkItemTitle,
                request.WorkItemType,
                request.SprintId,
                request.AssignedUserId,
                creatorUserId,
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