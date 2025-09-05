using Microsoft.Extensions.Logging;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class ConsoleEmailSender : IEmailSender
    {
        private readonly ILogger<ConsoleEmailSender> _logger;

        public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            _logger.LogInformation(
                $"\nEmail sent to: {toEmail}\n" +
                $"Subject: {subject}\n" +
                $"Message: {message}"
            );

            return Task.CompletedTask;
        }
    }
}
