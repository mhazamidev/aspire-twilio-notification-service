using BuildingBlocks.Messaging.Messaging.Abstractions;
using BuildingBlocks.Messaging.Messaging.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, RabbitMqEventBus>();

        return services;
    }
}
