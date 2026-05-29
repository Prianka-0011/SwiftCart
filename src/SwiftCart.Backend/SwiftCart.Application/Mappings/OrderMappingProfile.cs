using System;
using AutoMapper;
using SwiftCart.Application.Orders.Dto;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrdersResponseDto>();
    }

}
