using System.Collections.Generic;

namespace SwiftCart.Application.Dto;

public class RegisterUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
   
    public string Password { get; set; } = string.Empty;
    public List<AddressDto>? Addresses { get; set; }
}
