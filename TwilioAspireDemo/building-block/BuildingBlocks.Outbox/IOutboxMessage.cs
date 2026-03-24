namespace BuildingBlocks.Outbox;

public interface IOutboxMessage
{
    Guid Id { get; }
    string Payload { get; }
    string RoutingKey { get; }
    bool IsProcessed { get; set; }
    DateTime CreatedAt { get; }
}
