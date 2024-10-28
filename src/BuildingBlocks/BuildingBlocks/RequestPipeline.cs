using Carter;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks;

public static class RequestPipeline
{
    public static void UseBuildingBlocks(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(options => { });
    }
}