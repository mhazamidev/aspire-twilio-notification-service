using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Twilio.Rest.Verify.V2.Service;

namespace Notification.Application.Channels;

public class OtpChannel(
    IOptions<TwilioConfigs> _options,
    ITwilioClientFactory _factory) : IOtpChannel
{
    public MessageChannel Channel => MessageChannel.Otp;

    public async Task SendAsync(NotificationMessage message)
    {
        _factory.Initialize();

        await VerificationResource.CreateAsync(
            to: message.Recipient,
            channel: "sms",
            pathServiceSid: _options.Value.VerifyServiceSid
            );
    }

    public async Task<bool> VerifyAsync(string phoneNumber, string code)
    {
        var result = await VerificationCheckResource.CreateAsync(
         to: phoneNumber,
         code: code,
         pathServiceSid: _options.Value.VerifyServiceSid
         );

        return result.Status == "approved";
    }
}
