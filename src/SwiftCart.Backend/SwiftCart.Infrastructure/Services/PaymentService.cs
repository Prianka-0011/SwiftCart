using Microsoft.Extensions.Configuration;
using Stripe;
using SwiftCart.Application.Interfaces.Services;

namespace SwiftCart.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;

        public PaymentService(IConfiguration config) 
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        }

        public async Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(
            long amountInCents, 
            string orderId,
            string? paymentIntentId = null)
        {
            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(paymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amountInCents,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    Metadata = new Dictionary<string, string> { { "OrderId", orderId } }
                };
                intent = await service.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amountInCents,
                     Metadata = new Dictionary<string, string> { { "OrderId", orderId } }
                };
                intent = await service.UpdateAsync(paymentIntentId, options);
            }

            return intent;
        }

        
    }
}