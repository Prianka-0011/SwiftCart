using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Dto.Category;
using SwiftCart.Application.Interfaces.Repositories;

namespace SwiftCart.Application.Categories.Queries;

public class GetAllCategoryQuery: IRequest<List<CategoryResponseDto>>
{
    public class Handler : IRequestHandler<GetAllCategoryQuery, List<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public Handler(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CategoryResponseDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return categories.Select(_mapper.Map<CategoryResponseDto>).ToList();
        }
    }
}
