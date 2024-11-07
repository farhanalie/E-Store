using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries;

public static class GetOrders
{
    public record Query : PaginationRequest, IQuery<ErrorOr<Result>>;

    public record Result : PaginatedResult<OrderDto>;

    public class Handler(IAppDbContext dbContext) : IQueryHandler<Query, ErrorOr<Result>>
    {
        public async Task<ErrorOr<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            int totalCount = await dbContext.Orders.CountAsync(cancellationToken);

            List<OrderDto> orders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .OrderBy(o => o.OrderName)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .Select(o => o.ToDto())
                .ToListAsync(cancellationToken);

            return new Result
            {
                Count = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Data = orders
            };
        }
    }
}
