namespace SprintManager.Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public string WorkItemTitle { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
