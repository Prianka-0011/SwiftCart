using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftCart.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    public required Cart Cart { get; set; }
    public required Product Product { get; set; }
}
