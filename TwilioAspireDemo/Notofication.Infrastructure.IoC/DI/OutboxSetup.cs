using BuildingBlocks.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Database;

namespace Notofication.Infrastructure.IoC.DI;

public static class OutboxSetup
{
    public static IServiceCollection AddOutboxWorker(this IServiceCollection services)
    {
        services.AddHostedService<OutboxWorker<NotificationDbContext, OutboxMessage>>();

        return services;
    }
}
