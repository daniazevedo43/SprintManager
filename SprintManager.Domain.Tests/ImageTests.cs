using SprintManager.Domain.Entities;

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
            Image image = new Image(workItemId, attachedByUserId, "image/jpeg", "image", "path");

            Assert.NotEqual(Guid.Empty, image.Id);
            Assert.Equal(workItemId, image.WorkItemId);
            Assert.Equal(attachedByUserId, image.AttachedByUserId);
            Assert.Equal("image/jpeg", image.ContentType);
            Assert.Equal("image", image.FileName);
            Assert.Equal("path", image.FilePath);
            Assert.Equal(DateTime.UtcNow.Date, image.AttachmentDate.Date);
        }

        // Test exception throwing when work item ID is null or empty
        [Fact]
        public void VerifyWorkItemId_ThrowsException_WhenWorkItemIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Image(Guid.Empty, Guid.NewGuid(), "image/jpeg", "image", "path")
            );

            Assert.Equal("Work item ID can't be null or empty. (Parameter 'workItemId')", exception.Message);
        }

        // Test exception throwing when user ID is null or empty
        [Fact]
        public void VerifyAttachedByUserId_ThrowsException_WhenAttachedByUserIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Image(Guid.NewGuid(), Guid.Empty, "image/jpeg", "image", "path")
            );

            Assert.Equal("User ID can't be null or empty. (Parameter 'attachedByUserId')", exception.Message);
        }
    }
}