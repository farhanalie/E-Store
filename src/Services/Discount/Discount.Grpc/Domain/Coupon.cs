﻿namespace Discount.Grpc.Domain;

public class Coupon
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}
