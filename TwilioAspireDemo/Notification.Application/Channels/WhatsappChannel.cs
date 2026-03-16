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

public class WhatsappChannel(
    IOptions<TwilioConfigs> _options,
    ITwilioClientFactory _factory) : INotificationChannel
{
    public MessageChannel Channel => MessageChannel.WhatsApp;

    public async Task SendAsync(NotificationMessage message)
    {
        _factory.Initialize();

        await MessageResource.CreateAsync(
            from: new PhoneNumber($"whatsapp:{_options.Value.FromNumber}"),
            to: new PhoneNumber($"whatsapp:{message.Recipient}"),
            body: message.Content);
    }
}
