using System;
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task  UpdateUserAsync( User user)
    {   
        
        await _context.SaveChangesAsync();
       
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> SaveChangeAsync()
    {
        return await  _context.SaveChangesAsync() > 0;
    }

     
}

 