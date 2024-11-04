using Discount.Grpc;

namespace Basket.API.Basket;

public static class StoreBasket
{
    public sealed record Request(ShoppingCart Cart);

    public sealed record Response(UserId UserId);

    public sealed record Command(ShoppingCart Cart) : ICommand<ErrorOr<Response>>;

    internal sealed class Handler(IBasketRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command, cancellationToken);

            await repository.Store(command.Cart, cancellationToken);
            return new Response(command.Cart.UserId);
        }

        private async Task DeductDiscount(Command command, CancellationToken cancellationToken)
        {
            foreach (var item in command.Cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest
                    { ProductId = item.ProductId.Value }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Cart).NotNull();
            RuleFor(x => x.Cart.UserId).NotEmpty();
            RuleFor(x => x.Cart.Items).NotEmpty();
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket",
                async (Request request, ISender sender) =>
                    await Handle(request.Adapt<Command>(), sender)
            );
        }
    }
}
