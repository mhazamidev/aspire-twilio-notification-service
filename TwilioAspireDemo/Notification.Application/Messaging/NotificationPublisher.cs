using BuildingBlocks.Contracts.DTO;
using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Messaging;
using Notification.Infrastructure.Persistence.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notification.Application.Messaging;

public class NotificationPublisher(IConnection connection) : INotificationPublisher
{
    public async Task PublishAsync(NotificationDto dto, string routingKey)
    {
        var channel = await connection.CreateChannelAsync();
        var envelope = new NotificationEnvelope
        {
            Payload = dto,
            RetryCount = 0
        };

        var json = JsonSerializer.Serialize(envelope);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: Exchanges.Notification,
            routingKey: routingKey,
            body: body);
    }

  
}
