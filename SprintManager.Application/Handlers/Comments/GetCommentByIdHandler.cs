using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Comments;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Comments
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetCommentByIdHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);

            if (comment == null) throw new SprintManagerNotFoundException($"Comment with ID {request?.Id} not found");

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}