using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SprintManager.Application.Interfaces;

namespace SprintManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly AuthMessageSenderOptions _options;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(_options.ApiKey))
            {
                throw new Exception("Null SendGridKey");
            }
            _logger.LogInformation($"DEBUG: SendGrid API Key length is {_options.ApiKey.Length}");
            await Execute(_options.ApiKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            //var response = await client.SendEmailAsync(msg);
            try
            {
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("Email sent to {ToEmail}. Status Code: {StatusCode}", toEmail, response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Body.ReadAsStringAsync();
                    _logger.LogError("SendGrid API returned an error. Status Code: {StatusCode}, Body: {Body}", response.StatusCode, body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while sending the email.");
            }
        }
    }
}