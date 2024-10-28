namespace Catalog.API.Products;

public static class GetProductById
{
    public sealed record Response(Product Product);

    public sealed record Query(ProductId Id) : IQuery<ErrorOr<Response>>;

    internal sealed class Handler(IQuerySession session) : IQueryHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            if (product is null)
            {
                return Errors.Product.NotFound(query.Id);
            }

            return new Response(product);
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}",
                async (ProductId id, ISender sender) => await Handle(new Query(id), sender));
        }
    }
}