using System.Reflection;
using BuildingBlocks.Behaviors;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly()!;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddCarter(new DependencyContextAssemblyCatalogCustom(assembly));
        return services;
    }

    private class DependencyContextAssemblyCatalogCustom(Assembly assembly) : DependencyContextAssemblyCatalog
    {
        public override IReadOnlyCollection<Assembly> GetAssemblies()
        {
            return new List<Assembly> { assembly };
        }
    }
}