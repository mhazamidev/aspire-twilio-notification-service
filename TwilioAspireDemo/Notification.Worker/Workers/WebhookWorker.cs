using Notification.Infrastructure.Persistence.Database;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Notification.Worker.Workers;

public class WebhookWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WebhookWorker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public WebhookWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<WebhookWorker> logger,
        IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

            var messages = await db.WebhookMessages
                .Where(x => !x.IsProcessed && x.RetryCount < 3)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();

                    var response = await client.PostAsync(
                        msg.Url,
                        new StringContent(msg.Payload, Encoding.UTF8, "application/json"),
                        stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        msg.IsProcessed = true;
                    }
                    else
                    {
                        msg.RetryCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Webhook failed: {Id}", msg.Id);
                    msg.RetryCount++;
                }
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(2000, stoppingToken);
        }
    }
}
