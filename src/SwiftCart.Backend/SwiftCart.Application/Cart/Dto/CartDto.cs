using System;
using System.Collections.Generic;

namespace SwiftCart.Application.Cart.Dto;

public class CartDto
{
    public Guid Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal? GrandTotal { get; set; }
}

public class RequestCartDto
{
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? LineTotal => Quantity * UnitPrice;
}
