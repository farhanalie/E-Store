namespace Ordering.Application.Orders.Commands;

public static class DeleteOrder
{
    public sealed record Command(OrderId Id) : ICommand<ErrorOr<Deleted>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }

    internal sealed class Handler(IAppDbContext dbContext) : ICommandHandler<Command, ErrorOr<Deleted>>
    {
        public async Task<ErrorOr<Deleted>> Handle(Command request, CancellationToken cancellationToken)
        {
            Order? order = await dbContext.Orders.FindAsync([request.Id], cancellationToken);

            if (order is null)
            {
                return Errors.Order.NotFound;
            }

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
