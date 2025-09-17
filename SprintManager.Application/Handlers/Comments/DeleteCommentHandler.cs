using MediatR;
using Microsoft.AspNetCore.Http;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentHandler(
            IHttpContextAccessor httpContextAccessor, 
            ICommentRepository commentRepository
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _commentRepository = commentRepository;
        }

        public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated.");

            var comment = await _commentRepository.GetByIdAsync(request.Id);

            if (comment == null) throw new SprintManagerNotFoundException($"Comment with ID {request?.Id} not found.");
            if (comment.UserId != userId) throw new UnauthorizedAccessException($"You can't delete comments made by other users.");

            await _commentRepository.DeleteAsync(comment);
        }
    }
}