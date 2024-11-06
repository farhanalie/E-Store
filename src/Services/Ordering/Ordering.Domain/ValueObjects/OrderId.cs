namespace Ordering.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct OrderId
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("OrderId cannot be empty.") : Validation.Ok;
    }
}
