using BuildingBlocks.Contracts.Contracts.Notification;
using BuildingBlocks.Utility;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.MessageLogs.Enums;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Database;
using Notification.Worker.Interfaces;

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

        var handler = _factory.GetHandler(messageChannel);

        await handler.HandleAsync(envelope.Payload);

        _db.ProcessedMessages.Add(new ProcessedMessage
        {
            MessageId = envelope.MessageId,
            ProcessedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}
