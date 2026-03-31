namespace Notification.Infrastructure.Webhooks;

public class WebhookPayload
{
    public string Event { get; set; }

    public Guid MessageId { get; set; }

    public string Recipient { get; set; }

    public string Channel { get; set; }

    public string Status { get; set; }
}
