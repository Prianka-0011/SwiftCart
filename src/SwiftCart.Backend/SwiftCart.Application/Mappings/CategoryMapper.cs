using System;
using SwiftCart.Application.Dto.Category;
using SwiftCart.Domain.Entities;


namespace SwiftCart.Application.Mappings;

public class CategoryMapper
{
    public static CategoryResponseDto MapToDo(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            Description = category.Description,
            SubCategories = category.SubCategories.Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                ParentCategoryId = x.ParentCategoryId,
                Description = x.Description,

            }).ToList()
        };
    }
}
