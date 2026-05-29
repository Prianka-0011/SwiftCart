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
using SwiftCart.Domain.Entities;

namespace SwiftCart.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IMediator mediator, IUserContextService userContext) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserContextService _userContext = userContext;

    

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

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrderByAdmin(Guid id)
    {
        if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

        var order = await _mediator.Send(new GetOrderByIdQuery { OrderId = id });
        if (order == null) return NotFound();

    

        return Ok(order);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _mediator.Send(new GetOrdersQuery());
        return Ok(orders);
    }


    [HttpPost("initiate")]
    [Authorize]
    public async Task<ActionResult<CheckoutResponseDto>> InitiateCheckout(InitiateCheckoutDto input)
    {
        if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

        var result = await _mediator.Send(new InitiateCheckoutCommand { UserId = userId, CheckoutDetails = input });
        return Ok(result);

    }

    [HttpPost("confirm-payment")]
public async Task<ActionResult> ConfirmPayment(ConfirmPaymentDto input)
{
    if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

    await _mediator.Send(new ConfirmPaymentCommand {    OrderId = input.OrderId, TransactionId = input.TransactionId });
    return Ok();
    
}
}