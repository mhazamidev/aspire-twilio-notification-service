using Microsoft.Extensions.Options;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;
using Twilio;

namespace Notification.Application.Factories;

public class TwilioClientFactory : ITwilioClientFactory
{
    private readonly TwilioConfigs _options;
    public TwilioClientFactory(IOptions<TwilioConfigs> options)
    {
        _options = options.Value;
    }
    public void Initialize()
    {
        TwilioClient.Init(_options.AccountSid, _options.AuthToken);
    }
}
