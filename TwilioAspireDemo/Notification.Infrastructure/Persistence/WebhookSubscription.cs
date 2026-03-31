namespace Notification.Infrastructure.Persistence;

public class WebhookSubscription
{
    public Guid Id { get; set; }
    public string EventType { get; set; } 
    public string Url { get; set; }
    public bool IsActive { get; set; } = true;
}
