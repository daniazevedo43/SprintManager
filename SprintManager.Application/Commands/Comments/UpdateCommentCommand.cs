using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Comments
{
    public class UpdateCommentCommand : IRequest<CommentDTO>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}