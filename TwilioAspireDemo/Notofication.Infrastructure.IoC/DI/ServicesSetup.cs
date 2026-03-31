using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Messaging;
using Notification.Application.Services;
using Notification.Application.Webhooks;
using Notification.Infrastructure.Persistence.Interfaces;
using Notification.Infrastructure.Webhooks;
using INotificationPublisher = Notification.Infrastructure.Persistence.Interfaces.INotificationPublisher;

namespace Notofication.Infrastructure.IoC.DI;

public static class ServicesSetup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<OtpGenerator>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationPublisher, NotificationPublisher>();
        services.AddScoped<IRetryHandler, RetryHandler>();
        services.AddScoped<IWebhookService, WebhookService>();

        return services;
    }
}
