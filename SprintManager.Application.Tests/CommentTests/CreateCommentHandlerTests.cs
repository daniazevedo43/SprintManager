using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Auth;
using SprintManager.Application.Commands.Comments;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Comments;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Tests.CommentTests
{
    public class CreateCommentHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateCommentHandler _handler;

        public CreateCommentHandlerTests()
        {
            // Create mocks for UserManager constructor's dependencies
            var mockUserStore = new Mock<IUserStore<User>>();
            var mockOptions = new Mock<IOptions<IdentityOptions>>();
            var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            var mockUserValidator = new List<IUserValidator<User>>
            {
                new Mock<IUserValidator<User>>().Object
            };
            var mockPasswordValidator = new List<IPasswordValidator<User>>
            {
                new Mock<IPasswordValidator<User>>().Object
            };
            var mockLookupNormalizer = new Mock<ILookupNormalizer>();
            var mockErrors = new Mock<IdentityErrorDescriber>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<UserManager<User>>>();

            // Initialize mocks for each test
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object,
                mockOptions.Object,
                mockPasswordHasher.Object,
                mockUserValidator,
                mockPasswordValidator,
                mockLookupNormalizer.Object,
                mockErrors.Object,
                mockServiceProvider.Object,
                mockLogger.Object
            );
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new CreateCommentHandler(
                _mockHttpContextAccessor.Object,
                _mockCommentRepository.Object, 
                _mockWorkItemRepository.Object,
                _mockUserManager.Object,
                _mockMapper.Object
            );
        }

        // Test handler - comment creation
        [Fact]
        public async Task Handle_CreatesComment_ReturnsCommentDTO()
        {
            var command = new CreateCommentCommand
            {
                WorkItemId = Guid.NewGuid(),
                Text = "Test comment"
            };

            var userId = Guid.NewGuid();

            var comment = new Comment(command.WorkItemId, userId, command.Text);
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
            
            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId)).ReturnsAsync(new WorkItem());
            _mockUserManager.Setup(r => r.FindByIdAsync(userId.ToString())).ReturnsAsync(new User());
            _mockCommentRepository.Setup(r => r.AddAsync(It.IsAny<Comment>())).Callback<Comment>(c => comment = c);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<CommentDTO>(It.IsAny<Comment>())).Returns(commentDTO);

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

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(userId.ToString()), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockCommentRepository.Verify(r => r.AddAsync(comment), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created comment.
            _mockMapper.Verify(m => m.Map<CommentDTO>(It.IsAny<Comment>()), Times.Once);
        }

        // Test exception throwing when user is not authenticated
        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new CreateCommentCommand
            {
                WorkItemId = Guid.NewGuid(),
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
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            var command = new CreateCommentCommand
            {
                WorkItemId = Guid.NewGuid(),
                Text = "Test comment"
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {command.WorkItemId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            var command = new CreateCommentCommand
            {
                WorkItemId = Guid.NewGuid(),
                Text = "Test comment"
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId)).ReturnsAsync(new WorkItem());
            _mockUserManager.Setup(r => r.FindByIdAsync(userId.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {userId} not found.", exception.Message);

            // Ensure HttpContextAccessor was called exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(r => r.FindByIdAsync(userId.ToString()), Times.Once);
        }
    }
}