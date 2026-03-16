using MediatR;

namespace Domain.SeedWork.Events;

public interface IDomainEvent : INotification
{
    DateTime CreatedAt { get; }
}
public abstract record class DomainEvent : Message, IDomainEvent
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
