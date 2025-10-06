using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Comments;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.CommentTests
{
    public class GetCommentByIdHandlerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetCommentByIdHandler _handler;

        public GetCommentByIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetCommentByIdHandler(_mockCommentRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsCommentDto()
        {
            var query = new GetCommentByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "Test comment");

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                WorkItemId = comment.WorkItemId,
                UserId = comment.UserId,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };

            // Repository's mock configuration
            _mockCommentRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(comment);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<CommentDto>(comment)).Returns(commentDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(commentDto.Id, result.Id);
            Assert.Equal(commentDto.WorkItemId, result.WorkItemId);
            Assert.Equal(commentDto.UserId, result.UserId);
            Assert.Equal(commentDto.Text, result.Text);
            Assert.Equal(commentDto.CreationDate, result.CreationDate);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockCommentRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);

            // Ensure the mapper's Map was called exactly once.
            _mockMapper.Verify(m => m.Map<CommentDto>(comment), Times.Once);
        }

        // Test exception throwing when comment is not found
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentIsNotFound()
        {
            var query = new GetCommentByIdQuery
            {
                Id = Guid.NewGuid()
            };

            // Repository's mock configuration
            _mockCommentRepository.Setup(r => r.GetByIdAsync(query.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Comment with ID {query.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockCommentRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);
        }
    }
}