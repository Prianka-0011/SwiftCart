using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftCart.Application.Dto;

public class UpdateUserDto
{
    public string? Username { get; set; }
 
    public List<AddressDto>? Addresses { get; set; }
}
