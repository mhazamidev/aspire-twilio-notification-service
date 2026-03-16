using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.ValueObjects;

namespace Notification.Domain.MessageLogs.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(NotificationMessage message);
    Task<NotificationMessage?> GetAsync(NotificationMessageId id);
}
