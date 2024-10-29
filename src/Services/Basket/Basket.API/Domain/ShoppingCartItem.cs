using BuildingBlocks.Identifiers;

namespace Basket.API.Domain;

public class ShoppingCartItem
{
    public int Quantity { get; set; } = default!;
    public string Color { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public ProductId ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
}