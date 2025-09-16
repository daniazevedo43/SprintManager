using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentDTO>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepository _commentRepository;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateCommentHandler(
            IHttpContextAccessor httpContextAccessor,
            ICommentRepository commentRepository, 
            IWorkItemRepository workItemRepository,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _commentRepository = commentRepository;
            _workItemRepository = workItemRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated.");

            var workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {request.WorkItemId} not found.");
            if (user == null) throw new SprintManagerNotFoundException($"User with ID {userId} not found.");

            var comment = new Comment(request.WorkItemId, userId, request.Text);

            await _commentRepository.AddAsync(comment);

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}