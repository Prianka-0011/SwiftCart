using System;
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;



namespace SwiftCart.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Category?> CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;

    }

    public async Task<bool> CategoryExist(string name)
    {
        return _context.Categories.Any(x => x.Name == name);

    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories.Include(x => x.SubCategories).FirstOrDefaultAsync(x => x.Name == name);
    }




}
