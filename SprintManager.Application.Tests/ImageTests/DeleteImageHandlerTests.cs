using Moq;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ImageTests
{
    public class DeleteImageHandlerTests
    {
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly DeleteImageHandler _handler;

        public DeleteImageHandlerTests()
        {
            // Initialize mock for each test
            _mockImageRepository = new Mock<IImageRepository>();
            _mockFileStorageService = new Mock<IFileStorageService>();

            // Initialize handler injecting the mock
            _handler = new DeleteImageHandler(_mockImageRepository.Object, _mockFileStorageService.Object);
        }

        [Fact]
        public async Task Handle_GivenValid_DeletesImage()
        {
            var command = new DeleteImageCommand
            {
                Id = Guid.NewGuid(),
            };

            var image = new Image(
                Guid.NewGuid(), 
                Guid.NewGuid(), 
                "image/jpeg", 
                "test_image.jpg", 
                Path.Combine("images", "unique_test_file.jpg")
            );

            _mockImageRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(image);
            _mockFileStorageService.Setup(s => s.DeleteFile("Images", image.FileName));
            _mockImageRepository.Setup(r => r.DeleteAsync(image));

            await _handler.Handle(command, CancellationToken.None);

            _mockImageRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockFileStorageService.Verify(s => s.DeleteFile("Images", image.FileName), Times.Once);
            _mockImageRepository.Verify(r => r.DeleteAsync(image), Times.Once);
        }

        [Fact]
        public async Task VerifyImage_ThrowsException_WhenImageIsNotFound()
        {
            var command = new DeleteImageCommand
            {
                Id = Guid.NewGuid(),
            };

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Image with ID {command.Id} not found.", exception.Message);
        }
    }
}