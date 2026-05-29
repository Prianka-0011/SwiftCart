using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Orders.Dto;

namespace SwiftCart.Application.Orders.Queries;

public class GetOrdersQuery : IRequest<PagedResult<OrdersResponseDto?>>
{
    public class Handler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrdersQuery, PagedResult<OrdersResponseDto?>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<OrdersResponseDto?>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var dtoItems = _mapper.Map<List<OrdersResponseDto?>>(orders);

            return new PagedResult<OrdersResponseDto?>
            {
                Data = dtoItems,
                TotalCount = dtoItems.Count,
                Page = 1,
                PageSize = dtoItems.Count
            };
        }
    }
}
