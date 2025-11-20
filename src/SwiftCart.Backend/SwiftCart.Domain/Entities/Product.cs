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
    public Category Category { get; set; }

    public List<ProductImage> Images { get; set; } = [];
    public List<ProductReview> Reviews { get; set; } = [];
}

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public Product Product { get; set; }
}

public class ProductReview
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Product Product { get; set; }
    public User User { get; set; }
}