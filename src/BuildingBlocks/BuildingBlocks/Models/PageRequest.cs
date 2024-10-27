namespace BuildingBlocks.Models;

public abstract record PageRequest(int? PageNumber = 1, int? PageSize = 10);