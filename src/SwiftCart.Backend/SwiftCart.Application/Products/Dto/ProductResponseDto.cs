using System;

namespace SwiftCart.Application.Products.Dto;

public class ProductResponseDto
{
        public Guid Id { get; set; }
 public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? SKU { get; set; } = string.Empty;

        public decimal? Price { get; set; }
        
         
        public decimal? DiscountPrice { get; set; }
        
        public int StockQuantity { get; set; }
 
        public Guid? CategoryId { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
 
        public List<string>? Images { get; set; }
}
