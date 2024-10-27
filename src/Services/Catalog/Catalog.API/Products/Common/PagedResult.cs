namespace Catalog.API.Products.Common;

public abstract record PageRequest(int? PageNumber = 1, int? PageSize = 10);

public abstract record PageQuery<T>(int? PageNumber = 1, int? PageSize = 10) : IQuery<T> where T : IErrorOr;