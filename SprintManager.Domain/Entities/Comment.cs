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

        public Comment()
        {

        }

        public Comment(Guid workItemId, Guid userId, string text)
        {
            if(workItemId == Guid.Empty) throw new ArgumentException("Work item ID can't be null or empty.", nameof(workItemId));
            if(userId == Guid.Empty) throw new ArgumentException("User ID can't be null or empty.", nameof(userId));
            if(string.IsNullOrWhiteSpace(text)) throw new ArgumentException("A comment can't be null or empty.", nameof(text));
            if(text.Length > 500) throw new SprintManagerTooLongException("A comment can't exceed 500 characters.", 500, text.Length, nameof(text));

            Id = Guid.NewGuid();
            WorkItemId = workItemId;
            UserId = userId;
            Text = text;
            CreationDate = DateTime.UtcNow;
        }

        public void SetText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("A comment can't be null or empty.", nameof(text));
            if (text.Length > 500) throw new SprintManagerTooLongException("A comment can't exceed 500 characters.", 500, text.Length, nameof(text));

            Text = text;
        }
    }
}
