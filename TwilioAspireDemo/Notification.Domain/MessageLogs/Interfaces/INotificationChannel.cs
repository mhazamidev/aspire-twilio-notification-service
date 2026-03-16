using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;

namespace Notification.Domain.MessageLogs.Interfaces;

public interface INotificationChannel
{
    MessageChannel Channel { get; }
    Task SendAsync(NotificationMessage message);
}
