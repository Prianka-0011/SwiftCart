using System;
using SwiftCart.Domain.Enums;

namespace SwiftCart.Domain.Entities;
public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }

    public string? TransactionId { get; set; }
    public DateTime PaidAt { get; set; }

    public required Order Order { get; set; }
} 
