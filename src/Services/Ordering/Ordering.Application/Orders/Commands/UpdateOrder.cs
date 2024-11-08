using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.Commands;

public static class UpdateOrder
{
    public sealed record Command(
        OrderId OrderId,
        OrderName OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        OrderStatus Status) : ICommand<ErrorOr<Updated>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
            RuleFor(x => x.OrderName).NotEmpty().WithMessage("Name is required");
        }
    }

    internal sealed class Handler(IAppDbContext dbContext) : ICommandHandler<Command, ErrorOr<Updated>>
    {
        public async Task<ErrorOr<Updated>> Handle(Command request, CancellationToken cancellationToken)
        {
            Order? order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                return Errors.Order.NotFound;
            }

            order.Update(
                request.OrderName,
                request.ShippingAddress.ToEntity(),
                request.BillingAddress.ToEntity(),
                request.Payment.ToEntity(),
                request.Status);

            return Result.Updated;
        }
    }
}
