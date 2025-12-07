using System;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Reviews.Dto;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Reviews.Commands;
 

public class AddReviewCommand : IRequest<ReviewDto>
{
    public Guid UserId { get; set; }
    public AddReviewDto ReviewData { get; set; } = null!;

    public class Handler(IReviewRepository reviewRepo) : IRequestHandler<AddReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _repo = reviewRepo;

        public async Task<ReviewDto> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.HasUserReviewedProductAsync(request.UserId, request.ReviewData.ProductId))
            {
                throw new InvalidOperationException("You have already reviewed this product.");
            }

            var review = new ProductReview
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ProductId = request.ReviewData.ProductId,
                Rating = request.ReviewData.Rating,
                Comment = request.ReviewData.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddReviewAsync(review);

            return new ReviewDto
            {
                Id = review.Id,
            };
        }
    }
}
