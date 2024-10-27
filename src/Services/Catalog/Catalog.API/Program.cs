using BuildingBlocks;

var builder = WebApplication.CreateBuilder(args);

builder.AddBuildingBlocks();

builder.Services
    .AddMarten(options =>
    {
        // Establish the connection string to your Marten database
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
        // Specify that we want to use STJ as our serializer
        options.UseSystemTextJsonForSerialization();
    })
    .UseLightweightSessions();

var app = builder.Build();

app.UseBuildingBlocks();

app.MapGet("/", () => "Hello World!");

app.Run();