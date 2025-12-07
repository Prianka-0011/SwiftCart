using System;

using AutoMapper;
using MediatR;
using SwiftCart.Application.Cart.Dto;
using System.Linq;
using SwiftCart.Application.Interfaces.Repositories;
 
 

namespace SwiftCart.Application.Cart.Commands
{
    public class UpdateCartCommand : IRequest<CartDto>
    { 

        public  RequestCartDto Cart { get; set; } = null!;
        public Guid UserId { get; set; }
    
        public class Handler(ICartRepository repo, IMapper mapper ) : IRequestHandler<UpdateCartCommand,CartDto>
        {
            private readonly ICartRepository _repo = repo;
            private readonly IMapper _mapper = mapper;
 
            public async Task<CartDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
            {

                var userId = request.UserId ;


                var cart = await _repo.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    cart = new SwiftCart.Domain.Entities.Cart { Id = Guid.NewGuid(), UserId = userId };
                    await _repo.CreateCartAsync(cart);
                }

                foreach (var itemDto in request.Cart.Items)
                {
                    var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = itemDto.Quantity;
                        existingItem.UnitPrice = itemDto.UnitPrice;
                    }
                    else
                    {
                        cart.Items.Add(new SwiftCart.Domain.Entities.CartItem
                        {
                            ProductId = itemDto.ProductId,
                            Quantity = itemDto.Quantity,
                            UnitPrice = itemDto.UnitPrice,
                            Cart = cart,
                            Product = null!
                        });
                    }
                }

                await _repo.SaveChangesAsync();

                // Manual mapping to RequestCartDto to avoid missing AutoMapper configuration
                var result = new CartDto
                {
                    Id = cart.Id,
                    Items = cart.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = string.Empty,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                return result;
            }
 
        }
    }
}
