using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Products.Dto;

namespace SwiftCart.Application.Products.Queries;

public class GetProductQuery(Guid id) : IRequest<ProductResponseDto?>
{
    public Guid Id { get; } = id;

    public class Handler(IProductRepository repo, IMapper mapper) : IRequestHandler<GetProductQuery, ProductResponseDto?>
    {
        private readonly IProductRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductResponseDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetProductByIdAsync(request.Id);
            if (product == null) return null;
            return _mapper.Map<ProductResponseDto>(product);
        }
    }
}
