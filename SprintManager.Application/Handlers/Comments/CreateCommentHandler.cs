using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentDTO>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateCommentHandler(
            ICommentRepository commentRepository, 
            IWorkItemRepository workItemRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _commentRepository = commentRepository;
            _workItemRepository = workItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId);
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (workItem == null) throw new SprintManagerNotFoundException($"Work item with ID {request.WorkItemId} not found.");
            if (user == null) throw new SprintManagerNotFoundException($"User with ID {request.UserId} not found.");

            var comment = new Comment(request.WorkItemId, request.UserId, request.Text);

            await _commentRepository.AddAsync(comment);

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}