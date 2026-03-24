using BuildingBlocks.Outbox;

namespace Notification.Infrastructure.Persistence;

public class OutboxMessage : IOutboxMessage
{
    public Guid Id { get; set; }

    public string Payload { get; set; }

    public string RoutingKey { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime CreatedAt { get; set; }
}
