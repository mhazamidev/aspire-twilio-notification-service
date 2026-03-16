using Domain.SeedWork.Events;

namespace Domain.SeedWork;
public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
