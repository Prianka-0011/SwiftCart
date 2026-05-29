using System;
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application.Interfaces;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Order?> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
    { 
        var query = _context.Orders.AsQueryable();
        query = query.Where(o => o.UserId == userId);   
        query = query.OrderByDescending(o => o.CreatedAt);
        return await  query
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Images)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public void InitiateCheckout(Order order)
    {
         _context.Orders.Add(order);
         _context.SaveChanges();
    }

    public void ConfirmPayment(Order order)
    {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
}

