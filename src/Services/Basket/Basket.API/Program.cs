using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services
    .AddBuildingBlocks()
    .AddMarten(options =>
    {
        // Establish the connection string to your Marten database
        options.Connection(connectionString);

        // Specify that we want to use STJ as our serializer
        options.UseSystemTextJsonForSerialization();

        options.Schema.For<ShoppingCart>()
            .Identity(x => x.UserId);
    })
    .UseLightweightSessions();

string redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisConnectionString);

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!))
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        return handler;
    });

//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString)
    .AddRedis(redisConnectionString);

WebApplication app = builder.Build();

app.UseBuildingBlocks();

app.MapGet("/", () => "Hello World!");

app.Run();
