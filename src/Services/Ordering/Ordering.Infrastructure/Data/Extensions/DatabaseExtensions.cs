using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await SeedData(context);
    }

    #region Private Methods

    private static async Task SeedData(AppDbContext context)
    {
        await SeedCustomers(context);
        await SeedProducts(context);
        await SeedOrders(context);
    }

    private static async Task SeedCustomers(AppDbContext context)
    {
        if (!context.Customers.Any())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProducts(AppDbContext context)
    {
        if (!context.Products.Any())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrders(AppDbContext context)
    {
        if (!context.Orders.Any())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }

    #endregion
}
