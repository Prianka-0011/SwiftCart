using System;
using EmotiaMart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Infrastructure.Repositories;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Payment payment)
    {
        _context.Payments.Add(payment);
        _context.SaveChanges();
    }

    public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
    {
         return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
    {
         return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public void Update(Payment payment)
    {
        _context.Payments.Update(payment);
        _context.SaveChanges();
    }
}
