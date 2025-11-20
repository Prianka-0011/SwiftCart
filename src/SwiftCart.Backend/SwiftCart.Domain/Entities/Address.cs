using System;

namespace SwiftCart.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public User? User { get; set; }
}
 
