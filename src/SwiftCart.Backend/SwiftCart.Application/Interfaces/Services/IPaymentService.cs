using System;
using Stripe;
namespace SwiftCart.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(long amount, string orderId, string? existingIntentId = null);
}
