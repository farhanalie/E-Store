namespace BuildingBlocks.Pagination;

public abstract record PaginatedResult<TEntity> where TEntity : class
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public long Count { get; init; }
    public IEnumerable<TEntity> Data { get; init; }
}
