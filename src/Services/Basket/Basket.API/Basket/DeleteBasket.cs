namespace Basket.API.Basket;

public static class DeleteBasket
{
    public sealed record Command(UserId UserId) : ICommand<ErrorOr<Deleted>>;

    internal sealed class Handler(IBasketRepository repository) : ICommandHandler<Command, ErrorOr<Deleted>>
    {
        public async Task<ErrorOr<Deleted>> Handle(Command command, CancellationToken cancellationToken)
        {
            var deleted = await repository.Delete(command.UserId, cancellationToken);
            return deleted ? Result.Deleted : Errors.Basket.NotFound();
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class Endpoint : EndpointModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}",
                async (UserId userName, ISender sender) =>
                    await Handle(new Command(userName), sender, _ => Results.NoContent())
            );
        }
    }
}
