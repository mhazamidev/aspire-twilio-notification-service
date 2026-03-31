namespace Notification.Infrastructure.Webhooks;

public interface IWebhookService
{
    Task SendAsync(string eventType, WebhookPayload payload);
}
