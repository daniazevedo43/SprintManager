namespace SprintManager.Infrastructure.Services
{
    public class AuthMessageSenderOptions
    {
        public string? ApiKey { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }
}