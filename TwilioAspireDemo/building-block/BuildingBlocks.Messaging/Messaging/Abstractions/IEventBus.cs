namespace BuildingBlocks.Messaging.Messaging.Abstractions;

public interface IEventBus
{
    Task PublishAsync(string payload, string routingKey);
}
