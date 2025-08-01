using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.CommentTests
{
    public class CreateCommentHandlerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateCommentHandler _handler;

        public CreateCommentHandlerTests()
        {
            // Initialize mocks for each test
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateCommentHandler(_mockCommentRepository.Object, _mockMapper.Object);
        }

        // Test handler - basic comment creation
        [Fact]
        public async Task Handle_CreatesComment_ReturnsCommentDTO()
        {
            var command = new CreateCommentCommand
            {
                WorkItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Text = "Task completed!"
            };

            var comment = new Comment(command.WorkItemId, command.UserId, command.Text);
            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                WorkItemId = comment.WorkItemId,
                UserId = comment.UserId,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };

            // Repository's mock configuration
            _mockCommentRepository.Setup(r => r.AddAsync(It.IsAny<Comment>())).Callback<Comment>(c => comment = c);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<CommentDTO>(It.IsAny<Comment>())).Returns(commentDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(commentDTO.Id, result.Id);
            Assert.Equal(commentDTO.WorkItemId, result.WorkItemId);
            Assert.Equal(commentDTO.UserId, result.UserId);
            Assert.Equal(commentDTO.Text, result.Text);
            Assert.Equal(commentDTO.CreationDate, result.CreationDate);

            // Ensure AddAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.AddAsync(comment), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created comment.
            _mockMapper.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }
    }
}