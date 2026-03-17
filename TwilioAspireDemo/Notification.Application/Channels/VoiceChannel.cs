using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Polly;
using Polly.Retry;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notification.Application.Channels;

public class VoiceChannel : INotificationChannel
{
    private readonly IOptions<TwilioConfigs> _options;
    private readonly ITwilioClientFactory _factory;
    private readonly AsyncRetryPolicy _retryPolicy;

    public VoiceChannel(IOptions<TwilioConfigs> options, ITwilioClientFactory factory)
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
    public MessageChannel Channel => MessageChannel.Voice;

    public async Task SendAsync(NotificationMessage message)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var result = await CallResource.CreateAsync(
                url: new Uri("http://demo.twilio.com/docs/voice.xml"), //replace with your TwiML URL
                from: new PhoneNumber(_options.Value.FromNumber),
                to: new PhoneNumber(message.Recipient)
              );
        });      
    }
}
