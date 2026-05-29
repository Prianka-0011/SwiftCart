using EmotiaMart.Infrastructure.Data;
using SwiftCart.Domain.Entities;
using System.Text.Json;

namespace SwiftCart.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // 1. Ensure Database Exists
        context.Database.EnsureCreated();

        // 2. Check if data already exists to prevent duplicate seeding
        if (context.Products.Any()) return;

        // 3. Locate and Read the JSON file
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "seed-data.json");
        
        if (!File.Exists(filePath)) 
        {
            // Log this error in a real app, but for now we throw
            throw new FileNotFoundException($"Seed data file not found at: {filePath}");
        }

        var jsonString = File.ReadAllText(filePath);
        
        // 4. Deserialize JSON to DTOs
        var seedData = JsonSerializer.Deserialize<SeedDataDto>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (seedData == null) return;

        // ---------------------------------------------------------
        // A. SEED CATEGORIES
        // ---------------------------------------------------------
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(seedData.Categories);
            context.SaveChanges(); // Save now so IDs are valid for Products
        }

        // ---------------------------------------------------------
        // B. SEED PRODUCTS
        // ---------------------------------------------------------
        var products = new List<Product>();

        foreach (var dto in seedData.Products)
        {
            // Find the linked Category (Performance optimization: Check Local cache first)
            var category = context.Categories.Local.FirstOrDefault(c => c.Id == dto.CategoryId);
            
            // Fallback to DB if not in local memory (safety check)
            if (category == null) category = context.Categories.Find(dto.CategoryId);

            if (category != null)
            {
                var newProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    DiscountPrice = dto.Price > 100 ? dto.Price * 0.9m : null, // Auto-apply 10% discount logic
                    SKU = dto.SKU,
                    StockQuantity = dto.StockQuantity,
                    CategoryId = dto.CategoryId,
                    Category = category, // Required by EF Core entity
                    Color = dto.Color,
                    Size = dto.Size,
                    Weight = dto.Weight,
                    IsActive = dto.IsActive
                };

                // Map Images if they exist
                if (dto.Images != null && dto.Images.Count > 0)
                {
                    newProduct.Images = dto.Images.Select(img => new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = img.ImageUrl,
                        IsPrimary = img.IsPrimary,
                        Product = newProduct
                    }).ToList();
                }

                products.Add(newProduct);
            }
        }

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

// ---------------------------------------------------------
// DTOs (Data Transfer Objects) matching JSON structure
// ---------------------------------------------------------
public class SeedDataDto
{
    public List<Category> Categories { get; set; } = [];
    public List<ProductSeedDto> Products { get; set; } = [];
}

public class ProductSeedDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? Weight { get; set; }
    public bool IsActive { get; set; }
    public List<ProductImageSeedDto> Images { get; set; } = [];
}

public class ProductImageSeedDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}