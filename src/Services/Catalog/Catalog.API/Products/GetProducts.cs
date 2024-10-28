namespace Catalog.API.Products;

public static class GetProducts
{
    public sealed record Request : PageRequest;

    // Todo: refactor paged result
    public sealed record Response(IEnumerable<Product> Products, long PageNumber, long PageSize, long Count,
        bool HasNextPage,
        bool HasPreviousPage, long TotalItemCount);

    public sealed record Query : PageQuery<ErrorOr<Response>>;

    internal sealed class Handler(IQuerySession session) : IQueryHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return new Response(products, products.PageNumber, products.PageSize, products.Count, products.HasNextPage,
                products.HasPreviousPage, products.TotalItemCount);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products",
                async ([AsParameters] Request request, ISender sender) => await Handle(request.Adapt<Query>(), sender));
        }
    }
}