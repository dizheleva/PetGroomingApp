namespace PetGroomingApp.Web.Infrastructure.Services
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Logging;

    public class DummyEmailSender : IEmailSender
    {
        private readonly ILogger<DummyEmailSender> _logger;

        public DummyEmailSender(ILogger<DummyEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // In development, just log the email instead of sending it
            _logger.LogInformation("Email would be sent to {Email} with subject: {Subject}", email, subject);
            _logger.LogDebug("Email content: {Content}", htmlMessage);
            
            return Task.CompletedTask;
        }
    }
}

