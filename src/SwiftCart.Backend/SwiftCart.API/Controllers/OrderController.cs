using System.Security.Claims;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SwiftCart.Application.Orders.Commands;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Application.Orders.Queries;
using SwiftCart.Application.Interfaces;

namespace SwiftCart.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IMediator mediator, IUserContextService userContext) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserContextService _userContext = userContext;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        if (orderDto == null) return BadRequest("Invalid order payload.");

        if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

        var newId = await _mediator.Send(new CreateOrderCommand { UserId = userId, Order = orderDto });
        return CreatedAtAction(nameof(GetOrderById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserOrders()
    {
        if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

        var orders = await _mediator.Send(new GetUserOrdersQuery { UserId = userId });
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

        var order = await _mediator.Send(new GetOrderByIdQuery { OrderId = id });
        if (order == null) return NotFound();

        if (order.UserId != userId) return Forbid();

        return Ok(order);
    }


}
