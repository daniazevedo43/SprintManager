using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Images;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.ImageTests
{
    public class GetImageByIdHandlerTests
    {
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetImageByIdHandler _handler;

        public GetImageByIdHandlerTests()
        {
            // Initialize mocks for each test
            _mockImageRepository = new Mock<IImageRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetImageByIdHandler(_mockImageRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_GivenValidId_ReturnsImageDto()
        {
            var query = new GetImageByIdQuery
            {
                Id = Guid.NewGuid()
            };

            var image = new Image(
                Guid.NewGuid(), Guid.NewGuid(), "test_image/jpg",
                "test_image.jpg",
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

            // Repository's mock configuration
            _mockImageRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(image);

            // Mapper's mock configuration
            _mockMapper.Setup(m => m.Map<ImageDto>(image)).Returns(imageDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(imageDto.Id, result.Id);
            Assert.Equal(imageDto.WorkItemId, result.WorkItemId);
            Assert.Equal(imageDto.UserId, result.UserId);
            Assert.Equal(imageDto.ContentType, result.ContentType);
            Assert.Equal(imageDto.FileName, result.FileName);
            Assert.Equal(imageDto.FilePath, result.FilePath);
            Assert.Equal(imageDto.AttachmentDate, result.AttachmentDate);

            // Ensure GetByIdAsync was called exactly once with the correct ID.
            _mockImageRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);

            // Ensure the mapper's Map was called exactly once.
            _mockMapper.Verify(m => m.Map<ImageDto>(image), Times.Once);
        }

        // Test exception throwing when image is not found
        [Fact]
        public async Task VerifyImage_ThrowsException_WhenImageIsNotFound()
        {
            var query = new GetImageByIdQuery
            {
                Id = Guid.NewGuid()
            };

            // Repository's mock configuration
            _mockImageRepository.Setup(r => r.GetByIdAsync(query.Id));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal($"Image with ID {query.Id} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockImageRepository.Verify(p => p.GetByIdAsync(query.Id), Times.Once);
        }
    }
}