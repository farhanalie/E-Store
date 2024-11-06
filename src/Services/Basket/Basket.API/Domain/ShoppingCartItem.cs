namespace Basket.API.Domain;

public class ShoppingCartItem
{
    public int Quantity { get; set; } = default!;
    public string Color { get; set; } = default!;
    public decimal Price { get; set; }
    public ProductId ProductId { get; set; }
    public string ProductName { get; set; } = default!;
}
