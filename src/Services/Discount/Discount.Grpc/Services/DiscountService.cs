using Discount.Grpc.Data;
using Discount.Grpc.Domain;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>() ??
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Coupon"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", coupon.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        Coupon? coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductId == request.ProductId);
        if (coupon is null)
        {
            return new CouponModel
            {
                ProductId = 0,
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };
        }

        logger.LogInformation("Discount is retrieved for ProductName: {ProductName}", coupon.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>() ??
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Coupon"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", coupon.ProductName);
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        Coupon? coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductId == request.ProductId) ??
                         throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully deleted. ProductId: {ProductId}", request.ProductId);

        return new DeleteDiscountResponse
        {
            Success = true
        };
    }
}
