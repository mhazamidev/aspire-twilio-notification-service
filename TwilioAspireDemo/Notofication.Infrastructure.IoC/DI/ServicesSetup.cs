using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Messaging;
using Notification.Application.Services;
using Notification.Infrastructure.Persistence.Interfaces;

namespace Notofication.Infrastructure.IoC.DI;

public static class ServicesSetup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<OtpGenerator>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<NotificationPublisher>();

        return services;
    }
}
