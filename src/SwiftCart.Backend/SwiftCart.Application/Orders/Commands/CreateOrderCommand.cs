using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Domain.Entities;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Interfaces;

namespace SwiftCart.Application.Orders.Commands;

public class CreateOrderCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public CreateOrderDto Order { get; set; } = null!;

    public class Handler(IOrderRepository orderRepo, IAddressRepository addressRepo, ICartRepository cartRepo) : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepo = orderRepo;
        private readonly IAddressRepository _addressRepo = addressRepo;
        private readonly ICartRepository _cartRepo = cartRepo;

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(request.UserId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
                UserId = request.UserId,
                User = null!,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = Domain.Enums.OrderStatus.Pending,
                PaymentStatus = Domain.Enums.PaymentStatus.Pending
            };

            if (request.Order.ShippingAddressId.HasValue)
            {
                var savedAddress = await _addressRepo.GetByIdAsync(request.Order.ShippingAddressId.Value);
                if (savedAddress == null) throw new InvalidOperationException("Address not found.");
                if (savedAddress.UserId != request.UserId) throw new UnauthorizedAccessException("Address does not belong to user.");

                order.ShipToName = savedAddress.FullName;
                order.ShipStreet = savedAddress.Street;
                order.ShipCity = savedAddress.City;
                order.ShipState = savedAddress.State;
                order.ShipCountry = savedAddress.Country;
                order.ShipZipCode = savedAddress.ZipCode;
                order.ShippingAddressId = savedAddress.Id;
            }
            else if (request.Order.NewAddress != null)
            {
                var newDto = request.Order.NewAddress;

                order.ShipToName = newDto.FullName;
                order.ShipStreet = newDto.Street;
                order.ShipCity = newDto.City;
                order.ShipState = newDto.State;
                order.ShipCountry = newDto.Country;
                order.ShipZipCode = newDto.ZipCode;

                if (request.Order.SaveNewAddress)
                {
                    var newAddressEntity = new Address
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.UserId,
                        FullName = newDto.FullName,
                        Street = newDto.Street,
                        City = newDto.City,
                        State = newDto.State,
                        Country = newDto.Country,
                        ZipCode = newDto.ZipCode,
                        IsDefault = newDto?.IsDefault ?? false
                    };

                    await _addressRepo.CreateAsync(newAddressEntity);
                    order.ShippingAddressId = newAddressEntity.Id;
                }
            }
            else
            {
                throw new InvalidOperationException("Shipping address is required.");
            }

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Order = order,
                    ProductId = cartItem.ProductId,
                    Product = null!,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice
                };
                order.Items.Add(orderItem);
            }

            order.SubTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            order.ShippingCost = 0m;
            order.TaxAmount = 0m;
            order.TotalAmount = order.SubTotal + order.ShippingCost + order.TaxAmount;

            await _orderRepo.CreateOrderAsync(order);

            return order.Id;
        }
    }
}