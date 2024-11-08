namespace Ordering.Application.Orders.Queries;

public static class GetOrderByName
{
    public sealed record Query(OrderName Name) : IQuery<ErrorOr<IEnumerable<OrderDto>>>;


    internal sealed class Handler(IAppDbContext dbContext) : IQueryHandler<Query, ErrorOr<IEnumerable<OrderDto>>>
    {
        public async Task<ErrorOr<IEnumerable<OrderDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<OrderDto> orders = await dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .Where(o => ((string)o.OrderName).Contains(request.Name.Value))
                .OrderByDescending(o => o.OrderName)
                .Select(o => o.ToDto())
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}
