// using Microsoft.AspNetCore.Mvc;
// using SwiftCart.Application.Dto.Auth;
// using SwiftCart.Application.Interfaces;
// using SwiftCart.Application.Interfaces.Repositories;
// using SwiftCart.Domain.Entities;

// namespace SwiftCart.API.Controllers;

// [Route("api/[controller]")]
// [ApiController]
// public class AuthController : ControllerBase
// {
//     private readonly IUserRepository _userRepo;
//     private readonly ITokenService _tokenService;

//     public AuthController(IUserRepository userRepo, ITokenService tokenService)
//     {
//         _userRepo = userRepo;
//         _tokenService = tokenService;
//     }

//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginDto dto)
//     {
//         if (dto == null) return BadRequest("Invalid payload");

//         var user = await _userRepo.GetUserByEmailAsync(dto.Email);
//         if (user == null) return Unauthorized();

//         // Verify password using BCrypt (install BCrypt.Net-Next)
//         var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
//         if (!ok) return Unauthorized();

//         var token = _tokenService.CreateToken(user);

//         // Set JWT in HttpOnly cookie
//         var cookieOptions = new CookieOptions
//         {
//             HttpOnly = true,
//             Secure = Request.IsHttps,
//             SameSite = SameSiteMode.Strict,
//             Expires = DateTimeOffset.UtcNow.AddHours(4)
//         };
//         Response.Cookies.Append("jwt", token, cookieOptions);

//         return Ok(new { message = "Logged in" });
//     }

//     [HttpPost("logout")]
//     public IActionResult Logout()
//     {
//         Response.Cookies.Delete("jwt");
//         return NoContent();
//     }
// }
