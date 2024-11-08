using Ordering.Domain.Enums;

namespace Ordering.Application.Dtos;

public record OrderDto(
    OrderId Id,
    CustomerId CustomerId,
    OrderName OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    OrderStatus Status,
    List<OrderItemDto> OrderItems);
