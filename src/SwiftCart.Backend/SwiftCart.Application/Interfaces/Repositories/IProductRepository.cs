using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IProductRepository

{
    Task<Guid> CreateProductAsync(Product entity);
    Task<List<Product>> GetAllProductsAsync(string[]? categories, string? sort);
    Task<(List<Product> Items, int TotalCount)> GetAllProductsPagedAsync(string[]? categories, string? sort, int page, int pageSize);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<bool> UpdateProductAsync(Product entity);
    Task<bool> DeleteProductAsync(Guid id);
}
