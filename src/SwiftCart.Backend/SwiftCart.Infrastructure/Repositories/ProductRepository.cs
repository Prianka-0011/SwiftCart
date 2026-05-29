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
        return await _context.Products.Include(x => x.Images).Include(x => x.Category).FirstOrDefaultAsync(p => p.Id == id);
    }



    public async Task<List<Product>> GetAllProductsAsync(string[]? categories, string? sort)

    {
        var query = _context.Products.AsQueryable();
        if (categories != null && categories.Length > 0)
        {
            query = query.Where(x => x.Category != null && categories.Contains(x.Category.Name));
        }

         query = sort switch 
         {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
         };

        return await query.Include(x => x.Images).Include(x => x.Category).AsNoTracking().ToListAsync();
    }

    public async Task<(List<Product> Items, int TotalCount)> GetAllProductsPagedAsync(string[]? categories, string? sort, int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 12;

        var query = _context.Products.AsQueryable();
        if (categories != null && categories.Length > 0)
        {
            query = query.Where(x => x.Category != null && categories.Contains(x.Category.Name));
        }

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            _ => query.OrderBy(p => p.Id)
        };

        var total = await query.CountAsync();

        var items = await query
            .Include(x => x.Images)
            .Include(x => x.Category)
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
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
