using BuildingBlocks.Messaging;
using BuildingBlocks.Messaging.Messaging.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.Outbox;

public class OutboxWorker<TDbContext, TOutbox> : BackgroundService
    where TDbContext : DbContext
    where TOutbox : class, IOutboxMessage
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    private readonly ILogger<OutboxWorker<TDbContext, TOutbox>> _logger;
    private readonly IEventBus _eventBus;

    public OutboxWorker(
        IServiceScopeFactory scopeFactory,
        IConnection connection,
        ILogger<OutboxWorker<TDbContext, TOutbox>> logger,
        IEventBus eventBus)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
        _logger = logger;
        _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

            var messages = await db.Set<TOutbox>()
                .Where(x => !x.IsProcessed)
                .OrderBy(x => x.CreatedAt)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    await _eventBus.PublishAsync(msg.Payload, msg.RoutingKey);
                    msg.IsProcessed = true;
                }
                catch
                {
                    _logger.LogError("Failed to publish message with Id {MessageId}", msg.Id);
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}
