using Catalog.API.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddBuildingBlocks();

string connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services
    .AddMarten(options =>
    {
        // Establish the connection string to your Marten database
        options.Connection(connectionString);
        // Specify that we want to use STJ as our serializer
        options.UseSystemTextJsonForSerialization();
    })
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

WebApplication app = builder.Build();

app.UseBuildingBlocks();

app.MapGet("/", () => "Hello World!");

app.Run();
