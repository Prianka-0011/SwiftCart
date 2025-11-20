using System;
using System.Data.Common;

namespace SwiftCart.Application.Dto.Category;

public class CreateCategoryDto
{
        public required string Name { get; set; } 
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
