
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Dto.Category;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Mappings;
using SwiftCart.Domain.Entities;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository repo) : ControllerBase

    {
        private readonly ICategoryRepository _repo = repo;
        [HttpPost("create")]
        public async Task<ActionResult> CreateCategory( CreateCategoryDto category)
        {
            if (category == null) return BadRequest("Invalid category payload");
            if (await _repo.CategoryExist(category.Name)) return BadRequest("This category already exists");

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId
            };

            var result = await _repo.CreateCategoryAsync(newCategory);
            if (result == null) return BadRequest("Problem creating category");

            
            var response = new { id = newCategory.Id, name = newCategory.Name };
            return CreatedAtAction(nameof(GetCategory), new { name = newCategory.Name }, response);
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(string name)
        {
            if (String.IsNullOrEmpty(name)) return BadRequest("Invalid Request");
            var   result = await _repo.GetCategoryByNameAsync(name);
            if (  result == null) return NotFound();
            var   response = CategoryMapper.MapToDo(result);
            return Ok(response);

        }


    }
}
