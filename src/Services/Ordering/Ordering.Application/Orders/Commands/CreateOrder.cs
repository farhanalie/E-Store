namespace Ordering.Application.Orders.Commands;

public static class CreateOrder
{
    public sealed record Command(
        CustomerId CustomerId,
        OrderName OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        List<OrderItemDto> OrderItems)
        : ICommand<ErrorOr<OrderId>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.OrderName).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.CustomerId).NotNull().WithMessage("CustomerId is required");
            RuleFor(x => x.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");

            RuleForEach(e => e.OrderItems)
                .SetValidator(new OrderItemsValidator());
        }

        private class OrderItemsValidator : AbstractValidator<OrderItemDto>
        {
            public OrderItemsValidator()
            {
                RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
                RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
                RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
            }
        }
    }

    internal sealed class Handler(IAppDbContext dbContext) : ICommandHandler<Command, ErrorOr<OrderId>>
    {
        public async Task<ErrorOr<OrderId>> Handle(Command request, CancellationToken cancellationToken)
        {
            Order order = Order.Create(
                OrderId.New,
                request.CustomerId,
                request.OrderName,
                request.ShippingAddress.ToEntity(),
                request.BillingAddress.ToEntity(),
                request.Payment.ToEntity()
            );

            foreach (OrderItemDto orderItem in request.OrderItems)
            {
                order.Add(orderItem.ProductId, orderItem.Quantity, orderItem.Price);
            }

            await dbContext.Orders.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
