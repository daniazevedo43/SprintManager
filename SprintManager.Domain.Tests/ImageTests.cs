using SprintManager.Domain.Entities;

namespace SprintManager.Domain.Tests
{
    public class ImageTests
    {
        // Test image upload
        [Fact]
        public void Image_Constructor_WithValidData_CreatesImageSuccessfully()
        {
            var workItemId = Guid.NewGuid();
            var attachedByUserId = Guid.NewGuid();
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
    }
}