using Moq;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ImageTests
{
    public class RemoveImageHandlerTests
    {
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly RemoveImageHandler _handler;

        public RemoveImageHandlerTests()
        {
            // Initialize mock for each test
            _mockImageRepository = new Mock<IImageRepository>();
            _mockFileStorageService = new Mock<IFileStorageService>();

            // Initialize handler injecting the mock
            _handler = new RemoveImageHandler(_mockImageRepository.Object, _mockFileStorageService.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValid_DeletesImage()
        {
            var command = new RemoveImageCommand
            {
                Id = Guid.NewGuid(),
            };

            var image = new Image(
                Guid.NewGuid(), 
                Guid.NewGuid(),
                "test_image/jpeg", 
                "test_image.jpg",
                "test_path"
            );

            _mockImageRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(image);
            _mockFileStorageService.Setup(s => s.DeleteFile(image.FilePath));
            _mockImageRepository.Setup(r => r.DeleteAsync(image));

            await _handler.Handle(command, CancellationToken.None);

            _mockImageRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockFileStorageService.Verify(s => s.DeleteFile(image.FilePath), Times.Once);
            _mockImageRepository.Verify(r => r.DeleteAsync(image), Times.Once);
        }

        // Test exception throwing when image is not found
        [Fact]
        public async Task VerifyImage_ThrowsException_WhenImageIsNotFound()
        {
            var command = new RemoveImageCommand
            {
                Id = Guid.NewGuid(),
            };

            // Repository's Mock configuration
            _mockImageRepository.Setup(r => r.GetByIdAsync(command.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Image with ID {command.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockImageRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        }
    }
}