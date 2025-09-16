using AutoMapper;
using Moq;
using SprintManager.Application.DTOs;
using SprintManager.Application.Handlers.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Images;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Tests.ImageTests
{
    public class GetAllImagesHandlerTests
    {
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllImagesHandler _handler;

        public GetAllImagesHandlerTests()
        {
            // Initialize mocks for each test
            _mockImageRepository = new Mock<IImageRepository>();
            _mockMapper = new Mock<IMapper>();

            // Initialize handler injecting the mocks
            _handler = new GetAllImagesHandler(_mockImageRepository.Object, _mockMapper.Object);
        }

        // Test handler
        [Fact]
        public async Task Handle_ReturnsAllImages()
        {
            var query = new GetAllImagesQuery();

            var images = new List<Image>()
            {
                new Image(
                    Guid.NewGuid(), Guid.NewGuid(), "test_image/jpg", 
                    "test_image.jpg",
                    Path.Combine("test_path", "test_path_2.jpg")
                ),
                new Image(
                    Guid.NewGuid(), Guid.NewGuid(), "test_image/jpg",
                    "test_image_2.jpg",
                    Path.Combine("test_path", "test_path_2.jpg")
                )
            };

            var imagesDTOs = new List<ImageDTO>()
            {
                new ImageDTO
                {
                    Id = images[0].Id,
                    WorkItemId = images[0].WorkItemId,
                    UserId = images[0].UserId,
                    ContentType = images[0].ContentType,
                    FileName = images[0].FileName,
                    FilePath = images[0].FilePath,
                    AttachmentDate = images[0].AttachmentDate
                },
                new ImageDTO
                {
                    Id = images[1].Id,
                    WorkItemId = images[1].WorkItemId,
                    UserId = images[1].UserId,
                    ContentType = images[1].ContentType,
                    FileName = images[1].FileName,
                    FilePath = images[1].FilePath,
                    AttachmentDate = images[1].AttachmentDate
                },
            };

            // Repository's mock configuration
            _mockImageRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(images);

            // Mapper's mock configuration
            _mockMapper.Setup(mapper => mapper.Map<List<ImageDTO>>(images)).Returns(imagesDTOs);

            var result = await _handler.Handle(query, CancellationToken.None);

            for (int i = 0; i < imagesDTOs.Count; i++)
            {
                Assert.Equal(images[i].Id, result[i].Id);
                Assert.Equal(images[i].WorkItemId, result[i].WorkItemId);
                Assert.Equal(images[i].UserId, result[i].UserId);
                Assert.Equal(images[i].ContentType, result[i].ContentType);
                Assert.Equal(images[i].FileName, result[i].FileName);
                Assert.Equal(images[i].FilePath, result[i].FilePath);
                Assert.Equal(images[i].AttachmentDate, result[i].AttachmentDate);
            }

            // Ensure GetAllAsync was called exactly once.
            _mockImageRepository.Verify(r => r.GetAllAsync(), Times.Once);

            // Ensure the mapper's Map method was called exactly once.
            _mockMapper.Verify(m => m.Map<List<ImageDTO>>(images), Times.Once);
        }
    }
}