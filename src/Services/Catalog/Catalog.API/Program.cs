using Catalog.API.Data;

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

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

var app = builder.Build();

app.UseBuildingBlocks();

app.MapGet("/", () => "Hello World!");

app.Run();