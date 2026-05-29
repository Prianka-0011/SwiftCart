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
                        // If quantity is zero or negative, remove the item
                        if (itemDto.Quantity <= 0)
                        {
                            cart.Items.Remove(existingItem);
                        }
                        else
                        {
                            existingItem.Quantity = itemDto.Quantity;
                            existingItem.UnitPrice = itemDto.UnitPrice;
                        }
                    }
                    else
                    {
                        // Only add new items with positive quantity
                        if (itemDto.Quantity > 0)
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
                }

                // Remove any items that are not present in the incoming cart (this makes the update a full replace)
                var requestedIds = request.Cart.Items.Select(i => i.ProductId).ToHashSet();
                var toRemove = cart.Items.Where(ci => !requestedIds.Contains(ci.ProductId)).ToList();
                foreach (var rem in toRemove)
                {
                    cart.Items.Remove(rem);
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
