namespace SprintManager.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public RefreshToken()
        {
        }

        public RefreshToken(Guid userId, string token)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpirationDate = DateTime.UtcNow.AddDays(7);
        }
    }
}