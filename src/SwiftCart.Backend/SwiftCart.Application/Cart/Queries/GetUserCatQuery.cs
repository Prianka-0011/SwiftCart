using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Cart.Dto;
using SwiftCart.Application.Interfaces.Repositories;

namespace SwiftCart.Application.Cart.Queries;

public class GetUserCatQuery: IRequest<CartDto>
{
    public Guid UserId { get; set; }
    public class Handler(ICartRepository repo, IMapper mapper) : IRequestHandler<GetUserCatQuery, CartDto>
    {
        private readonly ICartRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<CartDto> Handle(GetUserCatQuery request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                return new CartDto();
            }

            return _mapper.Map<CartDto>(cart);
        }
    }

}
