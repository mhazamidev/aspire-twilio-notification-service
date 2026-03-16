using Domain.SeedWork.Events;

namespace Domain.SeedWork;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : StronglyTypedId<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot(TId id) : base(id) { }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}