using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Comments
{
    public class GetCommentByIdQuery : IRequest<CommentDTO>
    {
        public Guid Id { get; set; }
    }
}