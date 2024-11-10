using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Orders.Commands;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        CreateOrder.Command command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private CreateOrder.Command MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        AddressDto addressDto = new(message.FirstName, message.LastName, message.EmailAddress,
            message.AddressLine, message.Country, message.State, message.ZipCode);
        PaymentDto paymentDto = new(message.CardName, message.CardNumber, message.Expiration, message.CVV,
            message.PaymentMethod);
        OrderId orderId = OrderId.New;
        return new CreateOrder.Command(
            CustomerId.From(message.CustomerId),
            OrderName.From(message.UserId),
            addressDto,
            addressDto,
            paymentDto,
            [
                new OrderItemDto(orderId, ProductId.From(1), 2, 950),
                new OrderItemDto(orderId, ProductId.From(2), 1, 840)
            ]
        );
    }
}
