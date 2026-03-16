using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Channels;
using Notification.Application.Factories;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.Interfaces;

namespace Notofication.Infrastructure.IoC.DI;

public static class TwilioSetup
{
    public static IServiceCollection AddTwilio(this IServiceCollection services)
    {
        services.AddScoped<INotificationChannel, SmsChannel>();
        services.AddScoped<INotificationChannel, EmailChannel>();
        services.AddScoped<INotificationChannel, WhatsappChannel>();
        services.AddScoped<INotificationChannel, VoiceChannel>();
        services.AddScoped<IOtpChannel, OtpChannel>();
        services.AddSingleton<ITwilioClientFactory, TwilioClientFactory>();
        return services;
    }
}
