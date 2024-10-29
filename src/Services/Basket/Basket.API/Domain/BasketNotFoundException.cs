using BuildingBlocks.Exceptions;

namespace Basket.API.Domain;

public class BasketNotFoundException(UserId userId) : NotFoundException("Basket", userId);
