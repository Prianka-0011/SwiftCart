using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    void Add(Payment payment);
    void Update(Payment payment);
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
    Task<Payment?> GetByTransactionIdAsync(string transactionId);
}
