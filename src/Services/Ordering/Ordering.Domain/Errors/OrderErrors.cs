namespace Ordering.Domain.Errors;

public static class Errors
{
    public static class Order
    {
        public static Error NotFound => Error.NotFound("Order.NotFound", "Order was not found.");
    }
}
