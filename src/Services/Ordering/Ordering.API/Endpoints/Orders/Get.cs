using BuildingBlocks;
using MediatR;
using Ordering.Application.Orders.Queries;

namespace Ordering.API.Endpoints.Orders;

public class Get : EndpointModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders",
            async ([AsParameters] GetOrders.Query request, ISender sender) =>
            await Handle(request, sender)
        );
    }
}
