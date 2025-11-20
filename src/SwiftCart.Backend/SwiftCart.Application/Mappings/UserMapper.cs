using SwiftCart.Application.Dto;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Mappings
{
    public static class UserMapper
    {
      
        public static UserResponseDto MapToDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Addresses = user.Addresses.Select(a => new AddressDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    FullName = a.FullName,
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    ZipCode = a.ZipCode,
                    IsDefault = a.IsDefault
                }).ToList()
            };
        }
    }
}