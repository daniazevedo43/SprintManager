using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Comments
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public UpdateCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);

            if (comment == null) throw new SprintManagerNotFoundException($"Comment with ID {request?.Id} not found.");

            comment.SetText(request.Text);

            await _commentRepository.UpdateAsync(comment);

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}