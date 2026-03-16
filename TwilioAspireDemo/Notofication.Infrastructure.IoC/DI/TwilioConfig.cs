using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Infrastructure.Configurations;

namespace Notofication.Infrastructure.IoC.DI;

public static class TwilioConfig
{
    public static IServiceCollection AddTwilioConfig(this IServiceCollection services,IConfiguration config)
    {
        services.Configure<TwilioConfigs>(option =>
        {
            option.AccountSid = config.GetValue<string>("AccountSid") ?? string.Empty;
            option.AuthToken = config.GetValue<string>("AuthToken") ?? string.Empty;
            option.FromNumber = config.GetValue<string>("PhoneNumber") ?? string.Empty;
            option.SendGridApiKey = config.GetValue<string>("SendGridApiKey") ?? string.Empty;
            option.VerifyServiceSid = config.GetValue<string>("VerifyServiceSid") ?? string.Empty;
        });

        return services;
    }
}
