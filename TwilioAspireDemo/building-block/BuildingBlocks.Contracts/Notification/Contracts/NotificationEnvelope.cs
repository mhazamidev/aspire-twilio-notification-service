using BuildingBlocks.Contracts.DTO;

namespace BuildingBlocks.Contracts.Notification.Contracts;

public class NotificationEnvelope
{
    public NotificationDto Payload { get; set; }
    public int RetryCount { get; set; } = 0;
}
