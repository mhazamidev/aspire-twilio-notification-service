using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Utility;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Interfaces;

namespace Notification.Worker.Processors;

public class NotificationProcessor
{
    private readonly INotificationHandlerFactory _factory;
    private readonly ILogger<NotificationProcessor> _logger;

    public NotificationProcessor(
        INotificationHandlerFactory factory,
        ILogger<NotificationProcessor> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task ProcessAsync(NotificationEnvelope envelope)
    {
        MessageChannel messageChannel;

        try
        {
            messageChannel = envelope.Payload.Channel.GetEnumValue<MessageChannel>();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "MessageChannel Converting Error");
            throw;
        }

        var handler = _factory.GetHandler(messageChannel);

        await handler.HandleAsync(envelope.Payload);
    }
}
