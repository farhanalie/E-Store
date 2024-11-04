using Discount.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.AddBuildingBlocks();

var connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services
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

var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        return handler;
    });

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString)
    .AddRedis(redisConnectionString);

var app = builder.Build();

app.UseBuildingBlocks();

app.MapGet("/", () => "Hello World!");

app.Run();