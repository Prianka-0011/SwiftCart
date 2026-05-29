using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Categories.Commands;

public class CreateCategoryCommand :IRequest<Guid?>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public class Handler(ICategoryRepository repo, IMapper mapper) : IRequestHandler<CreateCategoryCommand, Guid?>
    {
        private readonly ICategoryRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid? > Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Category>(request);
            var  category = await _repo.CreateCategoryAsync(entity);
            return category?.Id;
        }

        
    }
}
