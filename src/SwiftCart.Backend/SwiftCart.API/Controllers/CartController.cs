using System.Security.Claims;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Cart.Commands;
using SwiftCart.Application.Cart.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SwiftCart.Application.Cart.Queries;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(IMediator mediator, SwiftCart.Application.Interfaces.IUserContextService userContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly SwiftCart.Application.Interfaces.IUserContextService _userContext = userContext;


        [HttpPatch("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCart([FromBody] RequestCartDto cartDto)
        {
            if (cartDto == null)
                return BadRequest("Invalid cart payload.");

            if (!_userContext.TryGetUserId(out var userId))
                return Unauthorized();


            var updatedCart = await _mediator.Send(new UpdateCartCommand { Cart = cartDto, UserId = userId });
            return Ok(updatedCart);
        }

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetCart()
        {
            if (!_userContext.TryGetUserId(out var userId))
                return Unauthorized();

            var cart = await _mediator.Send(new GetUserCatQuery { UserId = userId });
            return Ok(cart);
        }
    }
}
