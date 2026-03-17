using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Configurations;
using Polly;
using Polly.Retry;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Notification.Application.Channels;

public class EmailChannel : INotificationChannel
{
    private readonly SendGridClient _client;
    private readonly IOptions<TwilioConfigs> _options;
    private readonly AsyncRetryPolicy _retryPolicy;

    public EmailChannel(SendGridClient client, IOptions<TwilioConfigs> options)
    {
        _options = options;
        _client = client;

        _retryPolicy = Policy
          .Handle<Exception>()
          .WaitAndRetryAsync(
              3,
              retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
          );
    }

    public MessageChannel Channel => MessageChannel.Email;

    public async Task SendAsync(NotificationMessage message)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("test@example.com"),
                Subject = "Hello Email",
                PlainTextContent = "Email from Aspire project"
            };

            msg.AddTo(new EmailAddress("user@example.com"));

            var result = await _client.SendEmailAsync(msg);
        });
    }
}
