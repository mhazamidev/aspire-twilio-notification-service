using BuildingBlocks.Contracts.Contracts.Notification;
using BuildingBlocks.Messaging;
using BuildingBlocks.Messaging.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Persistence.Interfaces;
using System.Text;
using System.Text.Json;

namespace Notification.Application.Messaging;

public class RetryHandler : IRetryHandler
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<RetryHandler> _logger;

    public RetryHandler(IEventBus eventBus, ILogger<RetryHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task HandleAsync(NotificationEnvelope envelope)
    {
        envelope.RetryCount++;
        var newBody = JsonSerializer.Serialize(envelope);
        try
        {
            if (envelope.RetryCount <= 3)
            {
                _logger.LogWarning("Message sent to Retry queue");

                await _eventBus.PublishAsync(
                    newBody,
                    QueueNames.Retry);
            }
            else
            {
                _logger.LogError("Message moved to DLQ");
                await _eventBus.PublishAsync(
                    newBody,
                    RoutingKeys.Dlq);
            }
        }
        catch (Exception publishEx)
        {
            _logger.LogCritical(publishEx, "Failed to publish retry/DLQ");
        }
    }
}