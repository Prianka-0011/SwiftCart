using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SwiftCart.Domain.Enums;

namespace SwiftCart.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }

    public string ShipToName { get; set; } = string.Empty;
    public string ShipStreet { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipState { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipZipCode { get; set; } = string.Empty;

    public Guid? ShippingAddressId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingCost { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TaxAmount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required User User { get; set; }
    public Address? ShippingAddress { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}