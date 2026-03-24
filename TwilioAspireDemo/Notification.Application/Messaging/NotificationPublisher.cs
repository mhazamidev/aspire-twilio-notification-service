using BuildingBlocks.Contracts.Contracts.Notification;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Database;
using Notification.Infrastructure.Persistence.Interfaces;
using System.Text.Json;

namespace Notification.Application.Messaging;

public class NotificationPublisher(NotificationDbContext _dbContext) : INotificationPublisher
{
    public async Task PublishAsync(NotificationDto dto, string routingKey)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Payload = JsonSerializer.Serialize(dto),
            RoutingKey = routingKey,
            IsProcessed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.OutboxMessages.AddAsync(outboxMessage);

        await _dbContext.SaveChangesAsync();
    }
}
