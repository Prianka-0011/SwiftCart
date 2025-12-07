using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces;


public interface IOrderRepository
{
    Task<Order?> CreateOrderAsync(Order order);
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<List<Order>> GetAllOrdersAsync();
}


