using Domain.SeedWork;

namespace Notification.Domain.MessageLogs.ValueObjects;

public class NotificationMessageId : StronglyTypedId<NotificationMessageId>
{
    public NotificationMessageId(Guid value) : base(value)
    {
    }

    public static NotificationMessageId NewId() => new(Guid.NewGuid());
}
