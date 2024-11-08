namespace Ordering.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct OrderId
{
    public static OrderId New => From(Guid.NewGuid());

    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("OrderId cannot be empty.") : Validation.Ok;
    }
}
