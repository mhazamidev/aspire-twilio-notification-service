using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notification.Application.Channels;

public class VoiceChannel(
    IOptions<TwilioConfigs> _options,
    ITwilioClientFactory _factory) : INotificationChannel
{
    public MessageChannel Channel => MessageChannel.Voice;

    public async Task SendAsync(NotificationMessage message)
    {
        _factory.Initialize();

        var result = await CallResource.CreateAsync(
                  url: new Uri("http://demo.twilio.com/docs/voice.xml"), //replace with your TwiML URL
                  from: new PhoneNumber(_options.Value.FromNumber),
                  to: new PhoneNumber(message.Recipient)
              );

    }
}
