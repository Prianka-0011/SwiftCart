 using System;

namespace SwiftCart.Application.Orders.Dto
{
    public class InitiateCheckoutDto
    {
        // Address fields from your form
        public string ShipToName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class CheckoutResponseDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    public class ConfirmPaymentDto
    {
        public Guid OrderId { get; set; }
         public required string TransactionId { get; set; } 
    }
}