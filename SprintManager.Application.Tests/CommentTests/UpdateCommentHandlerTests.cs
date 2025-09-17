using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Tests.CommentTests
{
    public class UpdateCommentHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateCommentHandler _handler;

        public UpdateCommentHandlerTests()
        {
            // Initialize mocks for each test
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new UpdateCommentHandler(
                _mockHttpContextAccessor.Object,
                _mockCommentRepository.Object, 
                _mockMapper.Object
            );
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesComment_ReturnsCommentDTO()
        {
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
                Text = "Test comment"
            };

            var userId = Guid.NewGuid();

            var comment = new Comment(Guid.NewGuid(), userId, command.Text);
            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                WorkItemId = comment.WorkItemId,
                UserId = comment.UserId,
                Text = comment.Text,
                CreationDate = comment.CreationDate
            };

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(comment);
            _mockCommentRepository.Setup(r => r.UpdateAsync(comment));

            // Mapper's Mock configuration
            _mockMapper.Setup(mapper => mapper.Map<CommentDTO>(comment)).Returns(commentDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(commentDTO.Id, result.Id);
            Assert.Equal(commentDTO.WorkItemId, result.WorkItemId);
            Assert.Equal(commentDTO.UserId, result.UserId);
            Assert.Equal(commentDTO.Text, result.Text);
            Assert.Equal(commentDTO.CreationDate, result.CreationDate);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure UpdateAsync was called exactly once with the modified comment.
            _mockCommentRepository.Verify(r => r.UpdateAsync(comment), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified comment.
            _mockMapper.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }

        // Test exception throwing when user is not authenticated
        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
                Text = "Test comment"
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

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentIsNotFound()
        {
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
                Text = "Test comment"
            };

            var userId = Guid.NewGuid();

            // Repository's Mock configuration
            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            // Repository's Mock configuration
            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Comment with ID {command?.Id} not found.", exception.Message);

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
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
                Text = "Test comment"
            };

            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), command.Text);

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            _mockCommentRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(comment);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"You can't update comments made by other users.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
}