using System;
using MediatR;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Interfaces;

namespace SwiftCart.Application.Orders.Queries;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public Guid OrderId { get; set; }

    public class Handler(IOrderRepository repo) : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IOrderRepository _repo = repo;

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var o = await _repo.GetOrderByIdAsync(request.OrderId);
            if (o == null) return null;

            return new OrderDto
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
                    UnitPrice = i.UnitPrice,
                     Images = i.Product != null
                        ? (i.Product.Images != null ? i.Product.Images.Select(img => img.ImageUrl).ToList() : new List<string>())
                        : new List<string>()    

                }).ToList()
            };
        }
    }
}