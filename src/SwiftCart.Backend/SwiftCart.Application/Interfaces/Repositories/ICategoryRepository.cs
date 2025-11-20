using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> CreateCategoryAsync(Category category);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<bool> CategoryExist(string name);
}
