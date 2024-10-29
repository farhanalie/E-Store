namespace Basket.API.Domain;

public class ShoppingCart
{
    public UserId UserId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart(UserId userId)
    {
        UserId = userId;
    }

    //Required for Mapping
    public ShoppingCart()
    {
    }
}