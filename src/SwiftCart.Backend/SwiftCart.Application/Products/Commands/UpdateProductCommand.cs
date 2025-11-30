using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Products.Dto;

namespace SwiftCart.Application.Products.Commands;

public class UpdateProductCommand : IRequest<ProductResponseDto>
{
    public Guid ProductId { get; set; }
    public decimal? Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int? StockQuantity { get; set; }
    public Guid? CategoryId { get; set; }
    public bool? IsActive { get; set; }

    public class Handler(IProductRepository repo, IMapper mapper) : IRequestHandler<UpdateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        public async Task<ProductResponseDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetProductByIdAsync(request.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            if (request.Price != null)
            {
                existingProduct.Price = request.Price.Value;
            }

            if (request.DiscountPrice != null)
            {
                existingProduct.DiscountPrice = request.DiscountPrice.Value;
            }

            if (request.StockQuantity != null)
            {
                existingProduct.StockQuantity = request.StockQuantity.Value;
            }

            if (request.CategoryId != null)
            {
                existingProduct.CategoryId = request.CategoryId.Value;
            }

            if (request.IsActive != null)
            {
                existingProduct.IsActive = request.IsActive.Value;
            }

            await _repo.UpdateProductAsync(existingProduct);
            return _mapper.Map<ProductResponseDto>(existingProduct);
           
        }
    }

}