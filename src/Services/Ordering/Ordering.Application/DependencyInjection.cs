using System.Reflection;
using BuildingBlocks;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddBuildingBlocks();

        services.AddFeatureManagement();
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }

    public static void UseApplicationServices(this WebApplication app)
    {
        app.UseBuildingBlocks();
    }
}
