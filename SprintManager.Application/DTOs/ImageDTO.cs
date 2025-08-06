namespace SprintManager.Application.DTOs
{
    public class ImageDTO
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public Guid UserId { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime AttachmentDate { get; set; }
    }
}