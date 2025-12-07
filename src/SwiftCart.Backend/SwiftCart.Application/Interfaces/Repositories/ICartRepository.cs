using System;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<SwiftCart.Domain.Entities.Cart?> GetCartByUserIdAsync(Guid userId);
    Task CreateCartAsync(SwiftCart.Domain.Entities.Cart cart);
    Task SaveChangesAsync();
}
