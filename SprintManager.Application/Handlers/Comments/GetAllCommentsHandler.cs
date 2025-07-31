using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Comments;

namespace SprintManager.Application.Handlers.Comments
{
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, List<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetAllCommentsHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<List<CommentDTO>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetAllAsync();

            return _mapper.Map<List<CommentDTO>>(comments);
        }
    }
}