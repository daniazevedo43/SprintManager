using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Comments
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public UpdateCommentHandler(
            IHttpContextAccessor httpContextAccessor,
            ICommentRepository commentRepository, 
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated.");

            var comment = await _commentRepository.GetByIdAsync(request.Id);

            if (comment == null) throw new SprintManagerNotFoundException($"Comment with ID {request?.Id} not found.");
            if (comment.UserId != userId) throw new UnauthorizedAccessException($"You can't update comments made by other users.");

            comment.SetText(request.Text);

            await _commentRepository.UpdateAsync(comment);

            return _mapper.Map<CommentDto>(comment);
        }
    }
}