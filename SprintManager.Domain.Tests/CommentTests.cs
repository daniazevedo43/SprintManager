using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class CommentTests
    {
        // Test comment creation
        [Fact]
        public void Comment_Constructor_WithValidData_CreatesCommentSuccessfully()
        {
            Guid workItemId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Comment comment = new Comment(workItemId, userId, "Comment 1");

            Assert.NotEqual(Guid.Empty, comment.Id);
            Assert.Equal(workItemId, comment.WorkItemId);
            Assert.Equal(userId, comment.UserId);
            Assert.Equal("Comment 1", comment.Text);
            Assert.Equal(DateTime.UtcNow.Date, comment.CreationDate.Date);
        }

        // Test text change
        [Fact]
        public void SetText_UpdatesTextSuccessfully()
        {
            Comment comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), "Comment 1");

            comment.SetText("Comment 2");

            Assert.Equal("Comment 2", comment.Text);
        }

        // Test exception throwing when work item ID is null or empty
        [Fact]
        public void VerifyWorkItemId_ThrowsException_WhenWorkItemIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Comment(Guid.Empty, Guid.NewGuid(), "Comment 1")
            );

            Assert.Equal("Work item ID can't be null or empty. (Parameter 'workItemId')", exception.Message);
        }

        // Test exception throwing when user ID is null or empty
        [Fact]
        public void VerifyUserId_ThrowsException_WhenUserIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Comment(Guid.NewGuid(), Guid.Empty, "Comment 1")
            );

            Assert.Equal("User ID can't be null or empty. (Parameter 'userId')", exception.Message);
        }

        // Test exception throwing when text is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyText_ThrowsException_WhenTextIsNullOrEmpty(string text)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Comment(Guid.NewGuid(), Guid.NewGuid(), text)
            );

            Assert.Equal("A comment can't be null or empty. (Parameter 'text')", exception.Message);
        }

        // Test exception throwing when text is too long
        [Fact]
        public void VerifyText_ThrowsException_WhenTextIsTooLong()
        {
            string text = new string('C', 501);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new Comment(Guid.NewGuid(), Guid.NewGuid(), text)
            );

            Assert.Equal($"Comment is too long. (Max length '500') (Actual length '{text.Length}') (Parameter 'text')", exception.Message);
        }
    }
}