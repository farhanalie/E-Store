namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> Get(UserId userId,
        CancellationToken cancellationToken = default);

    Task<ShoppingCart> Store(ShoppingCart basket,
        CancellationToken cancellationToken = default);

    Task<bool> Delete(UserId userId,
        CancellationToken cancellationToken = default);
}