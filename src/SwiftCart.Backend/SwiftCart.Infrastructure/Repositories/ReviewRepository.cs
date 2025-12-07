using System;
using Microsoft.EntityFrameworkCore;
using EmotiaMart.Infrastructure.Data;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Infrastructure.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ProductReview> AddReviewAsync(ProductReview review)
    {
        _context.Set<ProductReview>().Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<List<ProductReview>> GetReviewsByProductIdAsync(Guid productId)
    {
        return await _context.Set<ProductReview>()
            .Include(x => x.User)
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId)
    {
        return await _context.Set<ProductReview>()
            .AnyAsync(x => x.UserId == userId && x.ProductId == productId);
    }
}
