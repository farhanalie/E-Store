using BuildingBlocks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBuildingBlocks();

        return services;
    }

    public static void UseApplicationServices(this WebApplication app)
    {
        app.UseBuildingBlocks();
    }
}
