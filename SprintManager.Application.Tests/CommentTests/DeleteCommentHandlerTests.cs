using Moq;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Xml.Linq;

namespace SprintManager.Application.Tests.CommentTests
{
    public class DeleteCommentHandlerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly DeleteCommentHandler _handler;

        public DeleteCommentHandlerTests() 
        {
            // Initialize mock for each test
            _mockCommentRepository = new Mock<ICommentRepository>();

            // Initialize handler injecting the mock
            _handler = new DeleteCommentHandler(_mockCommentRepository.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesComment()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "Task completed!");

            // Repository's mock configuration
            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(comment);
            _mockCommentRepository.Setup(r => r.DeleteAsync(comment));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.DeleteAsync(comment), Times.Once);
        }

        // Test exception throwing when comment is not found
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentIsNotFound()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            // Repository's mock configuration
            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Comment with ID {command.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
} 