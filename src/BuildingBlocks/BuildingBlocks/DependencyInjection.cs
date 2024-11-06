using System.Reflection;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions;
using Carter;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddBuildingBlocks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        Assembly assembly = Assembly.GetEntryAssembly()!;

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(assembly);

        builder.Services.AddCarter(new DependencyContextAssemblyCatalogCustom(assembly));
        return builder;
    }

    private sealed class DependencyContextAssemblyCatalogCustom(Assembly assembly) : DependencyContextAssemblyCatalog
    {
        public override IReadOnlyCollection<Assembly> GetAssemblies()
        {
            return new List<Assembly> { assembly };
        }
    }
}
