using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Tests.ImageTests
{
    public class AddImageHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly Mock<IFormFile> _mockFile;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddImageHandler _handler;

        public AddImageHandlerTests()
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
            _mockImageRepository = new Mock<IImageRepository>();
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
            _mockFileStorageService = new Mock<IFileStorageService>();
            _mockFile = new Mock<IFormFile>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new AddImageHandler(
                _mockHttpContextAccessor.Object,
                _mockImageRepository.Object, 
                _mockWorkItemRepository.Object,
                _mockUserManager.Object,
                _mockFileStorageService.Object, 
                _mockMapper.Object
             );
        }

        // Test handler
        [Fact]
        public async Task Handle_AddsImage_ReturnsImageDto()
        {
            _mockFile.Setup(f => f.ContentType).Returns("test_image/jpeg");
            _mockFile.Setup(f => f.FileName).Returns("test_image.jpg");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                Image = _mockFile.Object,
            };

            var userId = Guid.NewGuid();

            var image = new Image(
                command.WorkItemId,
                userId, 
                command.Image.ContentType,
                command.Image.FileName,
                Path.Combine("test_path", "test_path_2.jpg")
            );

            var imageDto = new ImageDto
            {
                Id = image.Id,
                WorkItemId = image.WorkItemId,
                UserId = image.UserId,
                ContentType = image.ContentType,
                FileName = image.FileName,
                FilePath = image.FilePath,
                AttachmentDate = image.AttachmentDate
            };

            _mockHttpContextAccessor
                .Setup(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId)).ReturnsAsync(new WorkItem());
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(new User());
            _mockFileStorageService.Setup(s => s.SaveFileAsync(_mockFile.Object, "test_sub_folder")).ReturnsAsync(image.FilePath);
            _mockImageRepository.Setup(r => r.AddAsync(It.IsAny<Image>())).Callback<Image>(i => image = i);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<ImageDto>(It.IsAny<Image>())).Returns(imageDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(imageDto.Id, result.Id);
            Assert.Equal(imageDto.WorkItemId, result.WorkItemId);
            Assert.Equal(imageDto.UserId, result.UserId);
            Assert.Equal(imageDto.ContentType, result.ContentType);
            Assert.Equal(imageDto.FileName, result.FileName);
            Assert.Equal(imageDto.FilePath, result.FilePath);
            Assert.Equal(imageDto.AttachmentDate, result.AttachmentDate);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(a => a.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);

            // Ensure SaveFileAsync was called exactly once.
            _mockFileStorageService.Setup(s => s.SaveFileAsync(_mockFile.Object, "test_sub_folder")).ReturnsAsync(image.FilePath);

            // Ensure AddAsync was called exactly once.
            _mockImageRepository.Verify(r => r.AddAsync(image), Times.Once);

            // Ensure the mapper's Map was called exactly once with the created comment.
            _mockMapper.Verify(m => m.Map<ImageDto>(It.IsAny<Image>()), Times.Once);
        }

        // Test exception throwing when user is not authenticated
        [Fact]
        public async Task VerifyUser_ThrowException_WhenAuthenticatedUserIsNotFound()
        {
            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                Image = _mockFile.Object,
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
                .Verify(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);
        }

        // Test exception throwing when file extension is not allowed
        [Fact]
        public async Task VerifyFile_ThrowsException_WhenFileIsNotAllowed()
        {
            _mockFile.Setup(f => f.FileName).Returns("test_image.pdf");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                Image = _mockFile.Object,
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerFileNotAllowedException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Contains("File extension not allowed.", exception.Message);
        
            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);
        }

        // Test exception throwing when work item is not found
        [Fact]
        public async Task VerifyWorkItem_ThrowsException_WhenWorkItemIsNotFound()
        {
            _mockFile.Setup(f => f.FileName).Returns("test_image.jpg");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                Image = _mockFile.Object,
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Work item with ID {command.WorkItemId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);
        }

        // Test exception throwing when user is not found
        [Fact]
        public async Task VerifyUser_ThrowsException_WhenUserIsNotFound()
        {
            _mockFile.Setup(f => f.FileName).Returns("test_image.jpg");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                Image = _mockFile.Object,
            };

            var userId = Guid.NewGuid();

            _mockHttpContextAccessor
                .Setup(r => r.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            _mockWorkItemRepository.Setup(r => r.GetByIdAsync(command.WorkItemId)).ReturnsAsync(new WorkItem());
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString()));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"User with ID {userId} not found.", exception.Message);

            // Ensure HttpContextAccesor was used exactly once.
            _mockHttpContextAccessor
                .Verify(r => r.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetByIdAsync(command.WorkItemId), Times.Once);

            // Ensure FindByIdAsync was called exactly once.
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        }
    }
}