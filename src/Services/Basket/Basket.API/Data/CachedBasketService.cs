using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache,
    ILogger<CachedBasketRepository> logger) : IBasketRepository
{
    private const string CacheKeyPrefix = "Basket:";

    public async Task<ShoppingCart> Get(UserId userId, CancellationToken cancellationToken = default)
    {
        // implement the method
        var cacheKey = CacheKeyPrefix + userId;
        var cachedBasket = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (cachedBasket is not null)
        {
            logger.LogInformation("Basket for user {UserName} was found in cache.", userId);
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        logger.LogInformation("Basket for user {UserName} was not found in cache.", userId);
        var basket = await repository.Get(userId, cancellationToken);
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> Store(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.Store(basket, cancellationToken);
        var cacheKey = CacheKeyPrefix + basket.UserId;
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<bool> Delete(UserId userId, CancellationToken cancellationToken = default)
    {
        await repository.Delete(userId, cancellationToken);
        var cacheKey = CacheKeyPrefix + userId;
        await cache.RemoveAsync(cacheKey, cancellationToken);
        return true;
    }
}
