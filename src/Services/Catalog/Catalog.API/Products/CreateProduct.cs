namespace Catalog.API.Products;

public static class CreateProduct
{
    public sealed record Request(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price
    );

    public sealed record Response(ProductId ProductId);

    public sealed record Command(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price
    ) : ICommand<ErrorOr<Response>>;

    internal sealed class Handler(IDocumentSession session) : ICommandHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new Response(product.Id);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.ImageFile).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (Request request, ISender sender) =>
                {
                    var command = request.Adapt<Command>();
                    var result = await sender.Send(command);

                    return result.Match(
                        success => Results.Created($"/products/{success.ProductId}", success),
                        _ => Results.Problem());
                });
        }
    }
}