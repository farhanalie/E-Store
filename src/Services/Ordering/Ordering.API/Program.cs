using Ordering.Application;
using Ordering.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

app.UseApplicationServices();
app.UseInfrastructureServices();

app.MapGet("/", () => "Hello World!");

app.Run();
