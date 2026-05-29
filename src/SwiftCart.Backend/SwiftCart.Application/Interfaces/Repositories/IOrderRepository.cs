using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces;


public interface IOrderRepository
{
    Task<Order?> CreateOrderAsync(Order order);
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<List<Order>> GetAllOrdersAsync();
    Task UpdateOrderAsync(Order order);
    void InitiateCheckout(Order order);
    void ConfirmPayment(Order order);
}


