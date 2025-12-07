using System;
using System.Collections.Generic;
using SwiftCart.Application.Dto;

namespace SwiftCart.Application.Orders.Dto;

public class OrderDto
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

    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

public class CreateOrderDto
{
    public Guid? ShippingAddressId { get; set; }
    public AddressDto? NewAddress { get; set; }
    public bool SaveNewAddress { get; set; }
}

 