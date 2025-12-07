 using System;
using System.Linq;
using System.Collections.Generic;
using MediatR;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Interfaces;

namespace SwiftCart.Application.Orders.Queries;

public class GetUserOrdersQuery : IRequest<List<OrderDto>>
{
    public Guid UserId { get; set; }

    public class Handler(IOrderRepository repo) : IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repo = repo;

        public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetOrdersByUserIdAsync(request.UserId);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                UserId = o.UserId,
                ShipToName = o.ShipToName,
                ShipStreet = o.ShipStreet,
                ShipCity = o.ShipCity,
                ShipState = o.ShipState,
                ShipCountry = o.ShipCountry,
                ShipZipCode = o.ShipZipCode,
                SubTotal = o.SubTotal,
                ShippingCost = o.ShippingCost,
                TaxAmount = o.TaxAmount,
                TotalAmount = o.TotalAmount,
                PaymentStatus = o.PaymentStatus.ToString(),
                OrderStatus = o.OrderStatus.ToString(),
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();
        }
    }
}