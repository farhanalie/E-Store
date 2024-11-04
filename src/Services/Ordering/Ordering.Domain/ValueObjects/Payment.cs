﻿namespace Ordering.Domain.ValueObjects;

public record Payment
{
    private Payment(string cardName, string cardNumber, string expiration, string cvv, string paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public string CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public string PaymentMethod { get; } = default!;

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, string paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfNotEqual(cvv.Length, 3, "CVV length should be 3.");
        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }
}
