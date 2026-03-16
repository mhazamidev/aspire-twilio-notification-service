using Domain.SeedWork.Exceptions;

namespace Domain.SeedWork;

public abstract class StronglyTypedId<T> : ValueObject
{
    public Guid Value { get; }

    protected StronglyTypedId(Guid value)
    {
        if (value == Guid.Empty)
            throw new BusinessRuleException("ID cannot be empty.");

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
