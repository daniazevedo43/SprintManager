using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CreateCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment(request.WorkItemId, request.UserId, request.Text);

            await _commentRepository.AddAsync(comment);

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}