
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Categories.Commands;
using SwiftCart.Application.Categories.Queries;
using SwiftCart.Application.Dto.Category;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Mappings;
using SwiftCart.Domain.Entities;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator, ICategoryRepository repo) : ControllerBase

    {
        private readonly IMediator _mediator = mediator;
        private readonly ICategoryRepository _repo = repo;



        [HttpPost("create")]
        public async Task<ActionResult> CreateCategory(CreateCategoryDto category)
        {
            if (category == null) return BadRequest("Invalid category payload");
            if (await _repo.CategoryExist(category.Name)) return BadRequest("This category already exists");

            var command = new CreateCategoryCommand
            {
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId
            };
            var newId = await _mediator.Send(command);
            if (newId == null) return StatusCode(500, "An error occurred while creating the category");
            return Ok(new { Id = newId });
        }


        //         [HttpGet("{name}")]
        //         public async Task<ActionResult<CategoryResponseDto>> GetCategory(string name)
        //         {
        //             if (String.IsNullOrEmpty(name)) return BadRequest("Invalid Request");
        //             var   result = await _repo.GetCategoryByNameAsync(name);
        //             if (  result == null) return NotFound();
        //             var   response = CategoryMapper.MapToDo(result);
        //             return Ok(response);

        //         }



        [HttpGet("getAll")]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {

            var response = await _mediator.Send(new GetAllCategoryQuery());

            return Ok(response);
        }
    }




}
