using MediatR;

namespace SprintManager.Application.Commands.Comments
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}