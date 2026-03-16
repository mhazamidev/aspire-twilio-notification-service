using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Notification.Application.Channels;

public class EmailChannel : INotificationChannel
{
    private readonly SendGridClient _client;
    private readonly IOptions<TwilioConfigs> _options;

    public EmailChannel(SendGridClient client, IOptions<TwilioConfigs> options)
    {
        _options = options;

        if (_client == null)
            _client = new SendGridClient(_options.Value.SendGridApiKey);
    }

    public MessageChannel Channel => MessageChannel.Email;

    public async Task SendAsync(NotificationMessage message)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("test@example.com"),
            Subject = "Hello Email",
            PlainTextContent = "Email from Aspire project"
        };

        msg.AddTo(new EmailAddress("user@example.com"));

        var result = await _client.SendEmailAsync(msg);
    }
}
