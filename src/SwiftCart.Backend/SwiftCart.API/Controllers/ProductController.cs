using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Products.Commands;
using SwiftCart.Application.Products.Dto;
using SwiftCart.Application.Products.Queries;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost]
        public async Task<ActionResult> CreateProduct(CreateProductDto productDto)
        {
            var id = await Mediator.Send(new CreateProductCommand { Product = productDto });

            return Ok(new { Id = id });
        }

        [HttpGet("getAll")]
        public async Task<ActionResult> GetProductsAsync()
        {
            var result = await Mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto productDto)
        {
            var command = new UpdateProductCommand
            {
                ProductId = id,
                Price = productDto.Price,
                DiscountPrice = productDto.DiscountPrice,
                StockQuantity = productDto.StockQuantity,
                CategoryId = productDto.CategoryId,
                IsActive = productDto.IsActive
            };

            var updatedProduct = await Mediator.Send(command);
            return Ok(updatedProduct );
        }
    }
}
