using BuildingBlocks.Contracts.Contracts.Notification;
using BuildingBlocks.Utility;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.MessageLogs.Enums;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Database;
using Notification.Infrastructure.Webhooks;
using Notification.Worker.Interfaces;
using System.Text.Json;

namespace Notification.Worker.Processors;

public class NotificationProcessor
{
    private readonly INotificationHandlerFactory _factory;
    private readonly ILogger<NotificationProcessor> _logger;
    private readonly NotificationDbContext _db;

    public NotificationProcessor(
        INotificationHandlerFactory factory,
        ILogger<NotificationProcessor> logger,
        NotificationDbContext db)
    {
        _factory = factory;
        _logger = logger;
        _db = db;
    }

    public async Task ProcessAsync(NotificationEnvelope envelope)
    {
        MessageChannel messageChannel;

        try
        {
            if (await _db.ProcessedMessages.AnyAsync(x => x.MessageId == envelope.MessageId))
            {
                _logger.LogWarning("Duplicate message ignored: {MessageId}", envelope.MessageId);
                return;
            }
            messageChannel = envelope.Payload.Channel.GetEnumValue<MessageChannel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MessageChannel Converting Error");
            throw;
        }

        try
        {
            var handler = _factory.GetHandler(messageChannel);

            await handler.HandleAsync(envelope.Payload);

            await _db.ProcessedMessages.AddAsync(new ProcessedMessage
            {
                MessageId = envelope.MessageId,
                ProcessedAt = DateTime.UtcNow
            });

            await ProcessWebhookAsync(WebhookEvents.NotificationSent, "Sent", envelope);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notification processing failed");

            await ProcessWebhookAsync(WebhookEvents.NotificationFailed, "Failed", envelope);

            throw;
        }
        await _db.SaveChangesAsync();

    }

    public async Task ProcessWebhookAsync(string eventType, string status, NotificationEnvelope envelope)
    {
        var hooks = await _db.WebhookSubscriptions
            .Where(x => x.EventType == eventType && x.IsActive)
            .ToListAsync();

        foreach (var hook in hooks)
        {
            await _db.WebhookMessages.AddAsync(new WebhookMessage
            {
                Id = Guid.NewGuid(),
                IsProcessed = false,
                CreatedAt = DateTime.UtcNow,
                RetryCount = 0,
                EventType = WebhookEvents.NotificationFailed,
                Url = hook.Url,
                Payload = JsonSerializer.Serialize(new WebhookPayload
                {
                    MessageId = envelope.MessageId,
                    Recipient = envelope.Payload.Recipient,
                    Channel = envelope.Payload.Channel,
                    Status = status
                })
            });
        }
    }
}
