using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Comments
{
    public class CreateCommentCommand : IRequest<CommentDTO>
    {
        public Guid WorkItemId { get; set; }
        public string Text { get; set; }
    }
}