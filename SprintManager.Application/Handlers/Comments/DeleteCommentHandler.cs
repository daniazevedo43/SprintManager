using MediatR;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);

            if (comment == null) throw new SprintManagerNotFoundException($"Comment with ID {request?.Id} not found.");

            await _commentRepository.DeleteAsync(comment);
        }
    }
}
