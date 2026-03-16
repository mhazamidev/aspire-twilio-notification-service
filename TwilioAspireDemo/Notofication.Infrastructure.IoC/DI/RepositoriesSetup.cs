using Microsoft.Extensions.DependencyInjection;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.Repositories;

namespace Notofication.Infrastructure.IoC.DI;

public static class RepositoriesSetup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}
