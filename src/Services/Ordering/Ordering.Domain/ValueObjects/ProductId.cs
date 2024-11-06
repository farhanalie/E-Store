namespace Ordering.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct ProductId
{
    private static Validation Validate(Guid value)
    {
        return value == Guid.Empty ? Validation.Invalid("Product Id cannot be empty") : Validation.Ok;
    }
}
