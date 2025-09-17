using Microsoft.AspNetCore.Http;
using Moq;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Tests.CommentTests
{
    public class DeleteCommentHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly DeleteCommentHandler _handler;

        public DeleteCommentHandlerTests() 
        {
            // Initialize mock for each test
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockCommentRepository = new Mock<ICommentRepository>();

            // Initialize handler injecting the mock
            _handler = new DeleteCommentHandler(
                _mockHttpContextAccessor.Object,
                _mockCommentRepository.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_DeletesComment()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            var userId = Guid.NewGuid();

            var comment = new Comment(Guid.NewGuid(), userId, "Test comment");

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(comment);
            _mockCommentRepository.Setup(r => r.DeleteAsync(comment));

            await _handler.Handle(command, CancellationToken.None);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure DeleteAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.DeleteAsync(comment), Times.Once);
        }

        // Test exception throwing when user is not authenticated
        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier));

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User not authenticated.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);
        }

        // Test exception throwing when comment is not found
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentIsNotFound()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Comment with ID {command.Id} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }

        // Test exception throwing when comment was not made by authenticated user
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentWasNotMadeByAuthenticatedUser()
        {
            var command = new DeleteCommentCommand
            {
                Id = Guid.NewGuid(),
            };

            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "Test comment");

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(comment);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"You can't delete comments made by other users.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
} 