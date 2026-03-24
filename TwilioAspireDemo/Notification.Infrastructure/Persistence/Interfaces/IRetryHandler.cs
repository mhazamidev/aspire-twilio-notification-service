using BuildingBlocks.Contracts.Contracts.Notification;

namespace Notification.Infrastructure.Persistence.Interfaces;

public interface IRetryHandler
{
    Task HandleAsync(NotificationEnvelope envelope);
}
