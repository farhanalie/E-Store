namespace Catalog.API.Products;

public class UpdateProduct
{
    public sealed record Request(ProductId Id, string Name, List<string> Category, string Description, string ImageFile,
        decimal Price);

    public sealed record Command(ProductId Id, string Name, List<string> Category, string Description, string ImageFile,
        decimal Price) : ICommand<ErrorOr<Updated>>;

    internal sealed class Handler(IDocumentSession session) : ICommandHandler<Command, ErrorOr<Updated>>
    {
        public async Task<ErrorOr<Updated>> Handle(Command command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                return Errors.Product.NotFound(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            await session.SaveChangesAsync(cancellationToken);

            return Result.Updated;
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.ImageFile).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public sealed class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{id}",
                async (ProductId id, Request request, ISender sender) =>
                {
                    if (id != request.Id)
                    {
                        return ProblemResult(Errors.Product.IdMismatch(id));
                    }

                    return await Handle(request.Adapt<Command>(), sender, _ => Results.NoContent());
                }
            );
        }
    }
}
