using Microsoft.Extensions.Options;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Polly;
using Polly.Retry;
using Twilio.Rest.Verify.V2.Service;

namespace Notification.Application.Channels;

public class OtpChannel : IOtpChannel
{
    private readonly IOptions<TwilioConfigs> _options;
    private readonly ITwilioClientFactory _factory;
    private readonly AsyncRetryPolicy _retryPolicy;

    public OtpChannel(IOptions<TwilioConfigs> options, ITwilioClientFactory factory)
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
    public MessageChannel Channel => MessageChannel.Otp;

    public async Task SendAsync(NotificationMessage message)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await VerificationResource.CreateAsync(
                to: message.Recipient,
                channel: "sms",
                pathServiceSid: _options.Value.VerifyServiceSid
            );
        });

    }

    public async Task<bool> VerifyAsync(string phoneNumber, string code)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var result = await VerificationCheckResource.CreateAsync(
                to: phoneNumber,
                code: code,
                pathServiceSid: _options.Value.VerifyServiceSid
                );

            return result.Status == "approved";
        });

        return false;
    }
}
