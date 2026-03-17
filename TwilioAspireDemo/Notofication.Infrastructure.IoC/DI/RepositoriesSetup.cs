using Microsoft.Extensions.DependencyInjection;
using Notification.Application.UoW;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.Repositories;
using Notification.Infrastructure.Persistence.UOW;

namespace Notofication.Infrastructure.IoC.DI;

public static class RepositoriesSetup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationUnitofWork, NotificationUnitofWork>();

        return services;
    }
}
