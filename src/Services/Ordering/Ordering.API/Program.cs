using Ordering.Application;
using Ordering.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseInfrastructureServices();

app.MapGet("/", () => "Hello World!");

app.Run();
