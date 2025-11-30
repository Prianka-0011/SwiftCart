using System;

namespace SwiftCart.Application.Products.Dto;

public class UpdateProductDto
{
    public decimal? Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int? StockQuantity { get; set; }
    public bool? IsActive { get; set; }
    public Guid? CategoryId { get; set; }
}
