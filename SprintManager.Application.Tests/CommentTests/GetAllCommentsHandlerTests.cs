using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Comments;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.CommentTests
{
    public class GetAllCommentsHandlerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllCommentsHandler _handler;

        public GetAllCommentsHandlerTests()
        {
            // Initialize mocks for each test
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllCommentsHandler(_mockCommentRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllComments()
        {
            var query = new GetAllCommentsQuery();

            var comments = new List<Comment>()
            {
                new Comment(
                    Guid.NewGuid(), Guid.NewGuid(), "Test comment"
                ),
                new Comment(
                    Guid.NewGuid(), Guid.NewGuid(), "Test comment 2"
                )
            };

            var commentsDtos = new List<CommentDto>()
            {
                new CommentDto
                {
                    Id = comments[0].Id,
                    WorkItemId = comments[0].WorkItemId,
                    UserId = comments[0].UserId,
                    Text = comments[0].Text,
                    CreationDate = comments[0].CreationDate
                },
                new CommentDto
                {
                    Id = comments[1].Id,
                    WorkItemId = comments[1].WorkItemId,
                    UserId = comments[1].UserId,
                    Text = comments[1].Text,
                    CreationDate = comments[1].CreationDate
                },
            };

            // Repository's mock configuration
            _mockCommentRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(comments);

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<CommentDto>>(comments)).Returns(commentsDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < commentsDtos.Count; i++)
            {
                Assert.Equal(comments[i].Id, result[i].Id);
                Assert.Equal(comments[i].WorkItemId, result[i].WorkItemId);
                Assert.Equal(comments[i].UserId, result[i].UserId);
                Assert.Equal(comments[i].Text, result[i].Text);
                Assert.Equal(comments[i].CreationDate, result[i].CreationDate);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<CommentDto>>(comments), Times.Once);
        }
    }
}