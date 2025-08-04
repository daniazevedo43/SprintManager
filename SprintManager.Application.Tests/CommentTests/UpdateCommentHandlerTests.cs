using AutoMapper;
using Moq;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.Commands.WorkItems;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.CommentTests
{
    public class UpdateCommentHandlerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateCommentHandler _handler;

        public UpdateCommentHandlerTests()
        {
            // Initialize mocks for each test
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new UpdateCommentHandler(_mockCommentRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_UpdatesComment_ReturnsCommentDTO()
        {
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
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

            // Repository's Mock configuration
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

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockCommentRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

            // Ensure UpdateAsync was called exactly once with the modified comment.
            _mockCommentRepository.Verify(r => r.UpdateAsync(comment), Times.Once);

            // Ensure the mapper's Map was called exactly once with the modified comment.
            _mockMapper.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyComment_ThrowsException_WhenCommentIsNotFound()
        {
            var command = new UpdateCommentCommand
            {
                Id = Guid.NewGuid(),
                WorkItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Text = "Task completed!"
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Comment with ID {command?.Id} not found.", exception.Message);
        }
    }
}