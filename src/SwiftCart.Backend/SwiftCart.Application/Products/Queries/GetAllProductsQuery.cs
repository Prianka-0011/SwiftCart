using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Products.Dto;
using SwiftCart.Application.Dto;

namespace SwiftCart.Application.Products.Queries;

public class GetAllProductsQuery(string[]? categories = null, string? sort = null, int page = 1, int pageSize = 12) : IRequest<PagedResult<ProductResponseDto>>
{
    public string[]? Categories { get; } = categories;
    public string? Sort { get; } = sort;
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;

    public class Handler(IProductRepository repo, IMapper mapper) : IRequestHandler<GetAllProductsQuery, PagedResult<ProductResponseDto>>
    {
        private readonly IProductRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var (items, total) = await _repo.GetAllProductsPagedAsync(request.Categories, request.Sort, request.Page, request.PageSize);
            var dtoItems = _mapper.Map<List<ProductResponseDto>>(items);

            return new PagedResult<ProductResponseDto>
            {
                Data = dtoItems,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
