namespace Ordering.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct CustomerId
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("CustomerId cannot be empty.") : Validation.Ok;
    }
}
