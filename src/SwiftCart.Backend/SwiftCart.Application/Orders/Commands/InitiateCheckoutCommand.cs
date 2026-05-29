using System;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Interfaces.Services;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Domain.Entities;
using SwiftCart.Domain.Enums;

namespace SwiftCart.Application.Orders.Commands;

public class InitiateCheckoutCommand: IRequest<CheckoutResponseDto>
{
    public Guid UserId { get; set; }
    public InitiateCheckoutDto CheckoutDetails { get; set; } = new InitiateCheckoutDto();
 
    public class Handler(IOrderRepository orderRepo, ICartRepository cartRepo, IPaymentService paymentService, IProductRepository productRepo, IPaymentRepository paymentRepo): IRequestHandler<InitiateCheckoutCommand, CheckoutResponseDto>
    {
        private readonly IOrderRepository _orderRepo = orderRepo;
        private readonly ICartRepository _cartRepo = cartRepo;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly IProductRepository _productRepo = productRepo;
        private readonly IPaymentRepository _paymentRepo = paymentRepo;


        public async Task<CheckoutResponseDto> Handle(InitiateCheckoutCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())  {
                throw new InvalidOperationException("Cart is empty.");
            };
            // 1. Calculate Items Total
            var orderItems = new List<OrderItem>();
            decimal subTotal = 0;

            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetProductByIdAsync(item.ProductId);
                if (product == null) throw new InvalidOperationException($"Product {item.ProductId} unavailable");

                decimal finalPrice = product.DiscountPrice ?? product.Price;

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    Product = product,
                    Order = null!,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = finalPrice
                };
                orderItems.Add(orderItem);
                subTotal += finalPrice * item.Quantity;
            }
 
            decimal shippingCost = 10.00m;
            decimal taxAmount = subTotal * 0.05m;
            decimal totalAmount = subTotal + shippingCost + taxAmount;

            var input = request.CheckoutDetails;
            var order = new Order
            {
                UserId = userId,
                User = null!,
                OrderNumber = DateTime.Now.Ticks.ToString(),
                ShipToName = input.ShipToName,
                ShipStreet = input.Street,
                ShipCity = input.City,
                ShipState = input.State,
                ShipCountry = input.Country,
                ShipZipCode = input.ZipCode,
                SubTotal = subTotal,
                ShippingCost = shippingCost,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                Items = orderItems
            };

              _orderRepo.InitiateCheckout(order);
             
            string? existingIntentId = null; 
            
            var intent = await _paymentService.CreateOrUpdatePaymentIntentAsync(
                (long)(totalAmount * 100), 
                order.Id.ToString(), 
                existingIntentId
            );

            
            var payment = new Payment
            {
                OrderId = order.Id,
                Order = order,
                Amount = totalAmount,
                Method = PaymentMethod.Card,
                Status = PaymentStatus.Pending,
                TransactionId = intent.Id,  
                PaidAt = DateTime.MinValue
            };
            _paymentRepo.Add(payment);
           

            return new CheckoutResponseDto
            {
                ClientSecret = intent.ClientSecret,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = totalAmount
            };
            
        }
    }

      

}
