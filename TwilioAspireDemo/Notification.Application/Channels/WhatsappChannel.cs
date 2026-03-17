using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Polly;
using Polly.Retry;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notification.Application.Channels;

public class WhatsappChannel : INotificationChannel
{
    private readonly IOptions<TwilioConfigs> _options;
    private readonly ITwilioClientFactory _factory;
    private readonly AsyncRetryPolicy _retryPolicy;

    public WhatsappChannel(IOptions<TwilioConfigs> options, ITwilioClientFactory factory)
    {
        _options = options;
        _factory = factory;
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
            );
        _factory.Initialize();
    }
    public MessageChannel Channel => MessageChannel.WhatsApp;

    public async Task SendAsync(NotificationMessage message)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await MessageResource.CreateAsync(
                from: new PhoneNumber($"whatsapp:{_options.Value.FromNumber}"),
                to: new PhoneNumber($"whatsapp:{message.Recipient}"),
                body: message.Content);
        });
    }
}
