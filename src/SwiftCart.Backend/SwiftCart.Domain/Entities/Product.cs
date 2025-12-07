using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftCart.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DiscountPrice { get; set; }

    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }

    public Guid CategoryId { get; set; }
    public required Category Category { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? Weight { get; set; }
    public bool IsActive { get; set; } = true;

    public List<ProductImage> Images { get; set; } = [];
    public List<ProductReview> Reviews { get; set; } = [];
}

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public required Product Product { get; set; }
}

 