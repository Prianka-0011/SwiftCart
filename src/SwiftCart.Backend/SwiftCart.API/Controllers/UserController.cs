using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Mappings;
using SwiftCart.Application.Users.Commands;
using SwiftCart.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository repo, IMediator mediator, IUserContextService userContext) : BaseController(mediator)
    {
        private readonly IUserRepository _repo = repo;
        private readonly IUserContextService _userContext = userContext;

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> RegisterUser(RegisterUserDto user)
        {
            if (user == null)
                return BadRequest("Invalid user payload.");
            var result = await Mediator.Send(new RegisterUserCommand
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            });
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) return NotFound();
            var response = UserMapper.MapToDto(user);
            return Ok(response);

        }

        [HttpPatch("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateUserDto user)
        {
            try
            {
                var updated = await Mediator.Send(new UpdateUserCommand { Email = email, User = user });
                if (!updated) return BadRequest("Failed to update user.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var token = await Mediator.Send(command);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(2),
                };

                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok(new { Message = "Login Successful" });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid Email or Password");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {

            Response.Cookies.Delete("jwt");
            return Ok();
        }

        [HttpGet("me")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<UserResponseDto>> Me()
        {

            if (!_userContext.TryGetUserId(out var userId))
            {

                return Unauthorized();
            }

            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            var response = UserMapper.MapToDto(user);
            return Ok(response);
        }


    }
}
