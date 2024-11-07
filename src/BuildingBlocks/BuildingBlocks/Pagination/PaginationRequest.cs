namespace BuildingBlocks.Pagination;

public record PaginationRequest
{
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}
