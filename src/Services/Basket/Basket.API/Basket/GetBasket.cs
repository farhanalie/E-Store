namespace Basket.API.Basket;

public static class GetBasket
{
    public sealed record Response(ShoppingCart Cart);

    public sealed record Query(UserId UserId) : IQuery<ErrorOr<Response>>;

    internal sealed class Handler(IBasketRepository repository) : IQueryHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var cart = await repository.Get(query.UserId, cancellationToken);
            return new Response(cart);
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}",
                async (UserId userName, ISender sender) => await Handle(new Query(userName), sender));
        }
    }
}
