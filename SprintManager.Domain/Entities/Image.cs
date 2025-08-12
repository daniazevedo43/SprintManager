using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class Image
    {
        public Guid Id { get; private set; }
        public Guid WorkItemId { get; private set; }
        public Guid UserId { get; private set; }
        public string ContentType { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public DateTime AttachmentDate { get; private set; }
        public WorkItem? WorkItem { get; private set; }
        public User? User { get; private set; }
             
        public Image()
        {

        }

        public Image(Guid workItemId, Guid userId, string contentType, string fileName, string filePath)
        {
            if (workItemId == Guid.Empty) throw new ArgumentNullException(nameof(workItemId), "Work item ID can't be null or empty.");
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "User ID can't be null or empty.");

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(fileName);
            
            if (!allowedExtensions.Contains(fileExtension)) throw new SprintManagerFileNotAllowedException($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", allowedExtensions)}");

            Id = Guid.NewGuid();
            WorkItemId = workItemId;
            UserId = userId;
            ContentType = contentType;
            FileName = fileName;
            FilePath = filePath;
            AttachmentDate = DateTime.UtcNow;
        }
    }
}