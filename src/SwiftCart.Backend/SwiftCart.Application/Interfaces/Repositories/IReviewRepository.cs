using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<ProductReview> AddReviewAsync(ProductReview review);
    Task<List<ProductReview>> GetReviewsByProductIdAsync(Guid productId);
    Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId);
}
