namespace Ordering.Domain.ValueObjects;

[ValueObject<string>]
public readonly partial struct OrderName
{
    private static Validation Validate(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? Validation.Invalid("Order name cannot be empty") : Validation.Ok;
    }
}
