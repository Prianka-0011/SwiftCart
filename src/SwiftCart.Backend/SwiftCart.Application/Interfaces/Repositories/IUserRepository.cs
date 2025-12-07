using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task  UpdateUserAsync(  User user);
    Task DeleteUserAsync(string email);
    Task<bool> SaveChangeAsync();
    
}

   public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
