using System;
using AutoMapper;
using MediatR;
 
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Products.Commands;

public class CreateProductCommand : IRequest<Guid>
{
    public CreateProductDto Product { get; set; } = null!;

    public class Handler(IProductRepository repo, IMapper mapper, IFileService fileService) : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        private readonly IFileService _fileService = fileService;

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request.Product);
          if (request.Product.ImageFiles != null && request.Product.ImageFiles.Count > 0)
    {
        foreach (var file in request.Product.ImageFiles)
        {
            // 1. Upload file
            string path = await _fileService.SaveFileAsync(file, "products");

            // 2. Create Entity
            var img = new ProductImage
            {
                ImageUrl = path,
                Product = entity,
                // Logic: Make the first image primary, others secondary
                IsPrimary = entity.Images.Count == 0 
            };

            entity.Images.Add(img);
        }
    }
            var newId = await _repo.CreateProductAsync(entity);
            return  newId;
        }
    }
}
