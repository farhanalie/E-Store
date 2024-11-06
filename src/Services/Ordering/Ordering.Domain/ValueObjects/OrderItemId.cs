namespace Ordering.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct OrderItemId
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("OrderItemId cannot be empty.") : Validation.Ok;
    }
}
