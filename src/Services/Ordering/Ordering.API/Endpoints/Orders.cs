using BuildingBlocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.Endpoints;

public class Orders : EndpointModule
{
    private const string Route = "/orders";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Route,
            async ([AsParameters] GetOrders.Query request, ISender sender) =>
            await Handle(request, sender)
        );

        app.MapGet($"{Route}/{{orderName}}",
            async (string orderName, ISender sender) =>
                await Handle(new GetOrderByName.Query(OrderName.From(orderName)), sender)
        );

        app.MapGet($"{Route}/customer/{{customerId}}",
            async (CustomerId customerId, ISender sender) =>
                await Handle(new GetOrdersByCustomer.Query(customerId), sender)
        );

        app.MapPost(Route,
            async ([FromBody] CreateOrder.Command request, ISender sender) =>
            await Handle(request, sender, response => Results.Created($"{Route}/{response}", response))
        );

        app.MapPut($"{Route}/{{orderId}}",
            async (OrderId orderId, [FromBody] UpdateOrder.Command request, ISender sender) =>
            await Handle(request with { OrderId = orderId }, sender, _ => Results.NoContent())
        );

        app.MapDelete($"{Route}/{{orderId}}",
            async (Guid orderId, ISender sender) =>
                await Handle(new DeleteOrder.Command(OrderId.From(orderId)), sender, _ => Results.NoContent())
        );
    }
}
