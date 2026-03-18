using BuildingBlocks.Contracts.DTO;

namespace Notification.Infrastructure.Persistence.Interfaces;

public interface INotificationPublisher
{
    Task PublishAsync(NotificationDto dto, string routingKey);
}
