namespace Catalog.API.Products;

public static class DeleteProduct
{
    public sealed record Command(ProductId Id) : ICommand<ErrorOr<Deleted>>;

    internal sealed class Handler(IDocumentSession session) : ICommandHandler<Command, ErrorOr<Deleted>>
    {
        public async Task<ErrorOr<Deleted>> Handle(Command command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product is null)
            {
                return Errors.Product.NotFound(command.Id);
            }

            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",
                async (ProductId id, ISender sender) =>
                    await Handle(new Command(id), sender, _ => Results.NoContent())
            );
        }
    }
}
