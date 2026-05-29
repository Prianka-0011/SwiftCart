using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Enums;

namespace SwiftCart.Application.Orders.Commands;

public class ConfirmPaymentCommand: IRequest<ActionResult>
{
    public Guid OrderId { get; set; }
    public required string TransactionId { get; set; }

    public class Handler(IOrderRepository orderRepo, IPaymentRepository paymentRepo): IRequestHandler<ConfirmPaymentCommand, ActionResult>
    {
        private readonly IOrderRepository _orderRepo = orderRepo;
        private readonly IPaymentRepository _paymentRepo = paymentRepo;

        public async Task<ActionResult> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
           var order = await _orderRepo.GetOrderByIdAsync(request.OrderId);

            if (order == null)
                return new NotFoundObjectResult("Order not found");
 
            if (order.PaymentStatus == PaymentStatus.Paid)
                return new OkObjectResult("Order already confirmed");
 
            order.PaymentStatus = PaymentStatus.Paid;
            order.OrderStatus = OrderStatus.Confirmed;  
 
            var payment = await _paymentRepo.GetByOrderIdAsync(order.Id);

            if (payment != null)
            {
                payment.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.UtcNow;
                _paymentRepo.Update(payment);
            }
 
            await _orderRepo.UpdateOrderAsync(order);

            return new OkObjectResult(new { message = "Order successfully confirmed" });
        }
    }

}
