using eStoreOnline.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace eStoreOnline.Infrastructure.Implementations;

public class SendGridEmailSender : IEmailSender
{
    private readonly EmailSenderConfiguration _options;
    private readonly ILogger _logger;

    public SendGridEmailSender(IOptions<EmailSenderConfiguration> optionsAccessor,
        ILogger<SendGridEmailSender> logger)
    {
        _options = optionsAccessor.Value;
        _logger = logger;
    }


    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(_options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_options.SenderEmail, _options.SenderName),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking for privacy
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
            ? $"Email to {toEmail} queued successfully!"
            : $"Failure Email to {toEmail}");
    }
}