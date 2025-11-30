using System;
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }



    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.Include(x => x.Images).AsNoTracking().ToListAsync();
    }

    public async Task<bool> UpdateProductAsync(Product entity)
    {
        _context.Products.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var existingProduct = await GetProductByIdAsync(id);
        if (existingProduct == null) return false;

        _context.Products.Remove(existingProduct);
        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<Guid> CreateProductAsync( Product entity)
    {
         _context.Products.Add(entity);
           await _context.SaveChangesAsync();
         return entity.Id;
    }
}
