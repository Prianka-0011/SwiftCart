using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Reviews.Commands;
using SwiftCart.Application.Reviews.Dto;
using SwiftCart.Application.Reviews.Queries;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator, IUserContextService userContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserContextService _userContext = userContext;

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            if (!_userContext.TryGetUserId(out var userId)) return Unauthorized();

            var result = await _mediator.Send(new AddReviewCommand { UserId = userId, ReviewData = dto });
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            var result = await _mediator.Send(new GetProductReviews(productId));
            return Ok(result);
        }
    }
}
