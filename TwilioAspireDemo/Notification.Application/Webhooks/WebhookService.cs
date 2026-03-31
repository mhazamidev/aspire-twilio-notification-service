using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Persistence.Database;
using Notification.Infrastructure.Webhooks;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace Notification.Application.Webhooks;

public class WebhookService : IWebhookService
{
    private readonly HttpClient _httpClient;
    private readonly NotificationDbContext _db;
    private readonly ILogger<WebhookService> _logger;

    public WebhookService(
        HttpClient httpClient,
        NotificationDbContext db,
        ILogger<WebhookService> logger)
    {
        _httpClient = httpClient;
        _db = db;
        _logger = logger;
    }

    public async Task SendAsync(string eventType, WebhookPayload payload)
    {
        var hooks = await _db.WebhookSubscriptions
            .Where(x => x.EventType == eventType && x.IsActive)
            .ToListAsync();

        foreach (var hook in hooks)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(hook.Url, payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Webhook failed: {Url}", hook.Url);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Webhook error: {Url}", hook.Url);
            }
        }
    }
}
