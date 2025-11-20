using System;

namespace SwiftCart.Application.Dto.Category;

public class CategoryResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public string? Description { get; set; }
    public List<CategoryResponseDto> SubCategories { get; set; } = [];

}
