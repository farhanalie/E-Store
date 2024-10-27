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
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
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
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.ImageFile).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (Request request, ISender sender) =>
                    await Handle(request.Adapt<Command>(), sender,
                        response => Results.Created($"/products/{response.ProductId}", response))
            );
        }
    }
}