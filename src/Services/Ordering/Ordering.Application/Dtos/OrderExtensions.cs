namespace Ordering.Application.Dtos;

public static class OrderExtensions
{
    public static OrderDto ToDto(this Order o)
    {
        return new OrderDto(
            o.Id,
            o.CustomerId,
            o.OrderName,
            new AddressDto(
                o.ShippingAddress.FirstName,
                o.ShippingAddress.LastName,
                o.ShippingAddress.EmailAddress,
                o.ShippingAddress.AddressLine,
                o.ShippingAddress.State,
                o.ShippingAddress.Country,
                o.ShippingAddress.ZipCode
            ),
            new AddressDto(
                o.ShippingAddress.FirstName,
                o.ShippingAddress.LastName,
                o.ShippingAddress.EmailAddress,
                o.ShippingAddress.AddressLine,
                o.ShippingAddress.State,
                o.ShippingAddress.Country,
                o.ShippingAddress.ZipCode
            ),
            new PaymentDto(
                o.Payment.CardName,
                o.Payment.CardNumber,
                o.Payment.Expiration,
                o.Payment.CVV,
                o.Payment.PaymentMethod
            ),
            o.Status,
            o.OrderItems.Select(oi => new OrderItemDto(
                oi.OrderId,
                oi.ProductId,
                oi.Quantity,
                oi.Price
            )).ToList()
        );
    }
}
