using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IProductRepository

{
    Task<Guid> CreateProductAsync(Product entity);
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<bool> UpdateProductAsync(Product entity);
    Task<bool> DeleteProductAsync(Guid id);
}
