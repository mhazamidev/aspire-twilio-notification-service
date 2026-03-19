using Notification.Domain.MessageLogs.Enums;

namespace Notification.Worker.Interfaces;

public interface INotificationHandlerFactory
{
    INotificationHandler GetHandler(MessageChannel channel);
}
