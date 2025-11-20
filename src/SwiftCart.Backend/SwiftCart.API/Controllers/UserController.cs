using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Mappings;
using SwiftCart.Domain.Entities;

namespace SwiftCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository repo) : ControllerBase
    {
        private readonly IUserRepository _repo = repo;

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            if (user == null)
                return BadRequest("Invalid user payload.");

            var existing = await _repo.GetUserByEmailAsync(user.Email);
            if (existing != null)
            {
                return BadRequest("User already registered.");
            }

            var result = await _repo.CreateUserAsync(user);
            var response = UserMapper.MapToDto(result);
            Console.WriteLine("User registered: " + result.PasswordHash);
            return Ok(response);
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
            var existing = await _repo.GetUserByEmailAsync(email);
            if (existing != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Username))
                    existing.Username = user.Username;
                if (user.Addresses != null && user.Addresses.Count > 0)
                {
                    foreach (var addr in user.Addresses)
                    {
                        if (addr.Id == null || addr.Id == Guid.Empty)
                        {
                            Console.WriteLine("Adding new address for user: " + existing.Email);
                            var newAddress = new Address
                            {
                            
                                UserId = existing.Id,
                                FullName = addr.FullName,
                                Street = addr.Street,
                                City = addr.City,
                                State = addr.State,
                                ZipCode = addr.ZipCode,
                                Country = addr.Country,
                                IsDefault = addr.IsDefault ?? false
                            };
                            existing.Addresses.Add(newAddress);
                        }
                        else
                        {
                            Console.WriteLine("Updating address for user: " + existing.Email);
                            var existingAddress = existing.Addresses.FirstOrDefault(a => a.Id == addr.Id);
                            if (existingAddress != null)
                            {
                                if (!string.IsNullOrWhiteSpace(addr.FullName)) existingAddress.FullName = addr.FullName;
                                if (!string.IsNullOrWhiteSpace(addr.Street)) existingAddress.Street = addr.Street;
                                if (!string.IsNullOrWhiteSpace(addr.City)) existingAddress.City = addr.City;
                                if (!string.IsNullOrWhiteSpace(addr.State)) existingAddress.State = addr.State;
                                if (!string.IsNullOrWhiteSpace(addr.ZipCode)) existingAddress.ZipCode = addr.ZipCode;
                                if (!string.IsNullOrWhiteSpace(addr.Country)) existingAddress.Country = addr.Country;
                                if (addr.IsDefault.HasValue) existingAddress.IsDefault = addr.IsDefault.Value;
                            }
                             
                        }
                    }
                }


                var result = await _repo.SaveChangeAsync();
                if(!result) return BadRequest("Failed to update user.");
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

    }
}
