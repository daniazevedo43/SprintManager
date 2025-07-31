namespace SprintManager.Application.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public string WorkItemName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}