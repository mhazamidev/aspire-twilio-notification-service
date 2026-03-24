namespace BuildingBlocks.Contracts.Contracts.Notification;

public class NotificationEnvelope
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public NotificationDto Payload { get; set; }
    public int RetryCount { get; set; } = 0;
    public string RoutingKey { get; set; }
}
