namespace Ordering.Application.Dtos;

public record OrderItemDto(OrderId OrderId, ProductId ProductId, int Quantity, decimal Price);
