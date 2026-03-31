namespace Notification.Infrastructure.Persistence;

public class WebhookMessage
{
    public Guid Id { get; set; }

    public string EventType { get; set; }

    public string Payload { get; set; }

    public string Url { get; set; }

    public int RetryCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsProcessed { get; set; }
}
