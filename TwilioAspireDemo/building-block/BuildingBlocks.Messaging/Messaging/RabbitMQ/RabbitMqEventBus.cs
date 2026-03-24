using BuildingBlocks.Messaging.Messaging.Abstractions;
using RabbitMQ.Client;
using System.Text;

namespace BuildingBlocks.Messaging.Messaging.RabbitMQ;

public class RabbitMqEventBus(IConnection _connection) : IEventBus
{
    public async Task PublishAsync(string payload, string routingKey)
    {
        var channel = await _connection.CreateChannelAsync();
        var body = Encoding.UTF8.GetBytes(payload);

        await channel.BasicPublishAsync(
            exchange: Exchanges.Notification,
            routingKey: routingKey,
            body: body);
    }
}
