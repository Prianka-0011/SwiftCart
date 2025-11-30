using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SwiftCart.Application.Dto
{
    public class CreateProductDto
    {
         
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;

        public decimal Price { get; set; }
        
         
        public decimal? DiscountPrice { get; set; }
        
        public int StockQuantity { get; set; }
 
        public Guid CategoryId { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }

        public List<IFormFile>? ImageFiles { get; set; }
    }
 
}