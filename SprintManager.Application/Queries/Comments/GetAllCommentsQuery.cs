using MediatR;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Queries.Comments
{
    public class GetAllCommentsQuery : IRequest<List<CommentDto>>
    {
    }
}