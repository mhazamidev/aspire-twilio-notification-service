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

public class SmsChannel(
    IOptions<TwilioConfigs> _options,
    ITwilioClientFactory _factory) : INotificationChannel
{
    public MessageChannel Channel => MessageChannel.Sms;

    public async Task SendAsync(NotificationMessage message)
    {
        _factory.Initialize();

        await MessageResource.CreateAsync(
            from: new PhoneNumber(_options.Value.FromNumber),
            to: new PhoneNumber(message.Recipient),
            body: message.Content);
    }
}
