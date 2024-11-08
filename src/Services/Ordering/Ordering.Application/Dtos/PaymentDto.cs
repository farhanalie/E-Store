namespace Ordering.Application.Dtos;

public record PaymentDto(string CardName, string CardNumber, string Expiration, string Cvv, int PaymentMethod);

public static class PaymentDtoExtensions
{
    public static Payment ToEntity(this PaymentDto paymentDto)
    {
        return Payment.From(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.Cvv,
            paymentDto.PaymentMethod);
    }
}
