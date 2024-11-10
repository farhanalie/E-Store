namespace Ordering.Domain.ValueObjects;

[ValueObject<int>]
public readonly partial struct ProductId
{
    private static Validation Validate(int value)
    {
        return value <= 0 ? Validation.Invalid("Product Id cannot be less than or equal to zero") : Validation.Ok;
    }
}
