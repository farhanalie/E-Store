namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart> Get(UserId userId,
        CancellationToken cancellationToken = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userId, cancellationToken);
        return basket ?? throw new BasketNotFoundException(userId);
    }

    public async Task<ShoppingCart> Store(ShoppingCart basket,
        CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> Delete(UserId userId,
        CancellationToken cancellationToken = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userId, cancellationToken);
        if (basket is null)
        {
            return false;
        }

        session.Delete(basket);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
