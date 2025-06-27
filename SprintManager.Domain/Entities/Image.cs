namespace SprintManager.Domain.Entities
{
    public class Image
    {
        public Guid Id { get; private set; }
        public Guid WorkItemId { get; private set; }
        public Guid AttachedByUserId { get; private set; }
        public string ContentType { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public DateTime AttachmentDate { get; private set; }

        public Image()
        {

        }

        public Image(Guid workItemId, Guid attachedByUserId, string contentType, string fileName, string filePath)
        {
            if (workItemId == Guid.Empty) throw new ArgumentException("This work item doesn't exist.", nameof(workItemId));
            if (attachedByUserId == Guid.Empty) throw new ArgumentException("This user doesn't exist.", nameof(attachedByUserId));

            Id = Guid.NewGuid();
            WorkItemId = workItemId;
            AttachedByUserId = attachedByUserId;
            ContentType = contentType;
            FileName = fileName;
            FilePath = filePath;
            AttachmentDate = DateTime.UtcNow;
        }
    }
}
