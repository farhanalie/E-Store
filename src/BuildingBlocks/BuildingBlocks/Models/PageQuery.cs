using BuildingBlocks.CQRS;
using ErrorOr;

namespace BuildingBlocks.Models;

public abstract record PageQuery<TResponse>(int? PageNumber = 1, int? PageSize = 10) : IQuery<TResponse>
    where TResponse : IErrorOr;