using Microsoft.Extensions.Hosting;

namespace Notofication.Infrastructure.IoC.DI;

public static class RabbitMQSetup
{
    public static IHostApplicationBuilder AddRabbitMQ(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMQClient("rabbitmq");

        return builder;
    }
}
