namespace Catalog.API.Products;

public class GetProductsByCategory
{
    public sealed record Response(IEnumerable<Product> Products);

    public sealed record Query(string Category) : IQuery<ErrorOr<Response>>;

    internal sealed class Handler(IQuerySession session) : IQueryHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(x => x.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            return new Response(products);
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",
                async (string category, ISender sender) => await Handle(new Query(category), sender));
        }
    }
}
