using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Products.Dto;

namespace SwiftCart.Application.Products.Queries;

public class GetAllProductsQuery : IRequest<List<ProductResponseDto>>
{
    public class Handler(IProductRepository repo,IMapper mapper) : IRequestHandler<GetAllProductsQuery, List<ProductResponseDto>>
    {
        private readonly IProductRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        public async Task<List<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var  data =  await _repo.GetAllProductsAsync();
            return _mapper.Map<List<ProductResponseDto>>(data);
        }
    }
}
