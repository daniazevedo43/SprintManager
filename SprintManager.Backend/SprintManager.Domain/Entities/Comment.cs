using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid WorkItemId { get; private set; }
        public Guid UserId { get; private set; }
        public string Text { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public WorkItem? WorkItem { get; private set; }
        public User? User { get; private set; }

        public Comment()
        {
        }

        public Comment(Guid workItemId, Guid userId, string text)
        {
            if(workItemId == Guid.Empty) throw new ArgumentNullException(nameof(workItemId), "Work item ID can't be null or empty.");
            if(userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "User ID can't be null or empty.");
            if(string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text), "A comment can't be null or empty.");
            if(text.Length > 500) throw new SprintManagerTooLongException("Comment is too long.", 500, text.Length, nameof(text));

            Id = Guid.NewGuid();
            WorkItemId = workItemId;
            UserId = userId;
            Text = text;
            CreationDate = DateTime.UtcNow;
        }

        public void SetText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text), "A comment can't be null or empty.");
            if (text.Length > 500) throw new SprintManagerTooLongException("Comment is too long.", 500, text.Length, nameof(text));

            Text = text;
            UpdateDate = DateTime.UtcNow;
        }
    }
}