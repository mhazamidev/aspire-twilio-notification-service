using Domain.SeedWork.Events;

namespace Domain.SeedWork;

public abstract class Entity<TId>
    where TId : StronglyTypedId<TId>
{
    public TId Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }



    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        // StronglyTypedId خودش equality درست دارد
        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !Equals(left, right);


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
