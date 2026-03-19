using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Interfaces;

namespace Notification.Worker.Factories;

public class NotificationHandlerFactory: INotificationHandlerFactory
{
    private readonly IEnumerable<INotificationHandler> _handlers;

    public NotificationHandlerFactory(IEnumerable<INotificationHandler> handlers)
    {
        _handlers = handlers;
    }

    public INotificationHandler GetHandler(MessageChannel channel)
    {
        return _handlers.First(x => x.Channel == channel);
    }
}
