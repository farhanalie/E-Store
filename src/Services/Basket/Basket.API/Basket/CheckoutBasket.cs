using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket;

public static class CheckoutBasket
{
    public sealed record Command(
        UserId UserId,
        Guid CustomerId,
        decimal TotalPrice,
        string FirstName,
        string LastName,
        string EmailAddress,
        string AddressLine,
        string Country,
        string State,
        string ZipCode,
        string CardName,
        string CardNumber,
        string Expiration,
        string CVV,
        int PaymentMethod
    ) : ICommand<ErrorOr<Success>>;

    internal sealed class Handler(
        IBasketRepository repository,
        IPublishEndpoint publishEndpoint) : ICommandHandler<Command, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(Command command, CancellationToken cancellationToken)
        {
            ShoppingCart basket = await repository.Get(command.UserId, cancellationToken);

            BasketCheckoutEvent basketCheckoutEvent = command.Adapt<BasketCheckoutEvent>();

            basketCheckoutEvent.TotalPrice = basket.TotalPrice;

            await publishEndpoint.Publish(basketCheckoutEvent, cancellationToken);

            await repository.Delete(command.UserId, cancellationToken);

            return Result.Success;
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotNull();
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout",
                async (Command request, ISender sender) =>
                    await Handle(request, sender)
            );
        }
    }
}
