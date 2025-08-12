using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ImageTests
{
    public class AddImageHandlerTests
    {
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly Mock<IFormFile> _mockFile;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddImageHandler _handler;

        public AddImageHandlerTests()
        {
            // Initialize mocks for each test
            _mockImageRepository = new Mock<IImageRepository>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _mockFile = new Mock<IFormFile>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new AddImageHandler(_mockImageRepository.Object, _mockFileStorageService.Object, _mockMapper.Object);
        }

        // Test handler - add image
        [Fact]
        public async Task Handle_AddsImage_ReturnsImageDTO()
        {
            _mockFile.Setup(f => f.FileName).Returns("test_image.jpg");
            _mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),   
                Image = _mockFile.Object,
            };

            var image = new Image(
                command.WorkItemId, 
                command.UserId, 
                command.Image.ContentType,
                command.Image.FileName,
                Path.Combine("images", "unique_test_file.jpg")
            );

            var imageDTO = new ImageDTO
            {
                Id = image.Id,
                WorkItemId = image.WorkItemId,
                UserId = image.UserId,
                ContentType = image.ContentType,
                FileName = image.FileName,
                FilePath = image.FilePath,
                AttachmentDate = image.AttachmentDate
            };

            // File storage service's Mock configuration
            _mockFileStorageService.Setup(s => s.SaveFileAsync(_mockFile.Object, "Images")).ReturnsAsync(image.FilePath);

            // Repository's Mock configuration
            _mockImageRepository.Setup(r => r.AddAsync(It.IsAny<Image>())).Callback<Image>(i => image = i);

            // Mapper's Mock configuration
            _mockMapper.Setup(m => m.Map<ImageDTO>(It.IsAny<Image>())).Returns(imageDTO);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(imageDTO.Id, result.Id);
            Assert.Equal(imageDTO.WorkItemId, result.WorkItemId);
            Assert.Equal(imageDTO.UserId, result.UserId);
            Assert.Equal(imageDTO.ContentType, result.ContentType);
            Assert.Equal(imageDTO.FileName, result.FileName);
            Assert.Equal(imageDTO.FilePath, result.FilePath);
            Assert.Equal(imageDTO.AttachmentDate, result.AttachmentDate);

            // Ensure SaveFileAsync was called exactly once.
            _mockFileStorageService.Verify(s => s.SaveFileAsync(_mockFile.Object, "Images"), Times.Once);

            // Ensure AddAsync was called exactly once.
            _mockImageRepository.Verify(r => r.AddAsync(image), Times.Once);

            // Ensure the mapper's Map was called exactly once with the attached image.
            _mockMapper.Verify(m => m.Map<ImageDTO>(image), Times.Once);
        }

        // Test exception throwing when file extension is not allowed
        [Fact]
        public async Task VerifyFile_ThrowsException_WhenFileIsNotAllowed()
        {
            _mockFile.Setup(f => f.FileName).Returns("test_image.pdf");

            var command = new AddImageCommand
            {
                WorkItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Image = _mockFile.Object,
            };

            var exception = await Assert.ThrowsAsync<SprintManagerFileNotAllowedException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Contains("File extension not allowed.", exception.Message);
        }
    }
}