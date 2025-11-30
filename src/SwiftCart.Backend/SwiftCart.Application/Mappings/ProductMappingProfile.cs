using System;
using AutoMapper;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Products.Dto;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        // CreateMap<Product, CreateProductDto>();
        // CreateMap<ProductResponseDto, Product>();
        CreateMap<Product, ProductResponseDto>().ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageUrl).ToList()));


    }
}
