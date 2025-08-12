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