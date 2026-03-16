using Domain.SeedWork;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.ValueObjects;

namespace Notification.Domain.MessageLogs.Entities;

public class NotificationMessage : AggregateRoot<NotificationMessageId>
{
    public string Recipient { get; private set; }

    public string Content { get; private set; }

    public MessageChannel Channel { get; private set; }

    public MessageStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    private NotificationMessage() : base(NotificationMessageId.NewId())
    {
    }

    private NotificationMessage(NotificationMessageId id, string recipient, string content, MessageChannel channel, MessageStatus pending, DateTime utcNow) : base(id)
    {
        Recipient = recipient;
        Content = content;
        Channel = channel;
        Status = pending;
        CreatedAt = utcNow;
    }

    public static NotificationMessage Create(string recipient, string content, MessageChannel channel)
    {
        return new NotificationMessage
        (
            NotificationMessageId.NewId(),
            recipient,
            content,
            channel,
            MessageStatus.Pending,
            DateTime.UtcNow
        );
    }

    public void MarkAsSent()
    {
        Status = MessageStatus.Sent;
    }

    public void MarkAsFailed()
    {
        Status = MessageStatus.Failed;
    }
}


