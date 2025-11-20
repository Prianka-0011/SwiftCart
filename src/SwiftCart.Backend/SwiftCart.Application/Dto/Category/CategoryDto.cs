using System;

namespace SwiftCart.Application.Dto.Category;

public class CategoryDto
{
    public Guid Id {get; set;}
    public required string Name { get; set; } 
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
   

}
