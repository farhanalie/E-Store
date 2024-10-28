using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BuildingBlocks;

public static class RequestPipeline
{
    public static void UseBuildingBlocks(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(options => { });

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    }
}