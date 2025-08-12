using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class ImageTests
    {
        // Test image upload
        [Fact]
        public void Image_Constructor_WithValidData_CreatesImageSuccessfully()
        {
            Guid workItemId = Guid.NewGuid();
            Guid attachedByUserId = Guid.NewGuid();
            var image = new Image(workItemId, attachedByUserId, "image/jpeg", "image.jpeg", "path");

            Assert.NotEqual(Guid.Empty, image.Id);
            Assert.Equal(workItemId, image.WorkItemId);
            Assert.Equal(attachedByUserId, image.UserId);
            Assert.Equal("image/jpeg", image.ContentType);
            Assert.Equal("image.jpeg", image.FileName);
            Assert.Equal("path", image.FilePath);
            Assert.Equal(DateTime.UtcNow.Date, image.AttachmentDate.Date);
        }

        // Test exception throwing when work item ID is null or empty
        [Fact]
        public void VerifyWorkItemId_ThrowsException_WhenWorkItemIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Image(Guid.Empty, Guid.NewGuid(), "image/jpeg", "image.jpeg", "path")
            );

            Assert.Equal("Work item ID can't be null or empty. (Parameter 'workItemId')", exception.Message);
        }

        // Test exception throwing when user ID is null or empty
        [Fact]
        public void VerifyUserId_ThrowsException_WhenAttachedByUserIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Image(Guid.NewGuid(), Guid.Empty, "image/jpeg", "image.jpeg", "path")
            );

            Assert.Equal("User ID can't be null or empty. (Parameter 'userId')", exception.Message);
        }

        // Test exception throwing when file extension is not allowed
        [Theory]
        [InlineData(".pdf")]
        [InlineData(".txt")]
        [InlineData(".svg")]
        public void VerifyContentType_ThrowsException_WhenFileIsNotAllowed(string extension)
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };

            var exception = Assert.Throws<SprintManagerFileNotAllowedException>(() =>
                new Image(Guid.NewGuid(), Guid.NewGuid(), $"image/{extension}", $"image.{extension}", "path")
            );

            Assert.Equal($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", allowedExtensions)}", exception.Message);
        }
    }
}