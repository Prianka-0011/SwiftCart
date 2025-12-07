using System;
using System.Linq;
using AutoMapper;
using SwiftCart.Application.Cart.Dto;

namespace SwiftCart.Application.Mappings;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<Domain.Entities.Cart, CartDto>()
            .ForMember(x => x.GrandTotal, opt => opt.MapFrom(src => src.Items != null ? src.Items.Sum(i => i.Quantity * i.UnitPrice) : 0))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

        CreateMap<Domain.Entities.CartItem, CartItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty));
        
     }
 
}
