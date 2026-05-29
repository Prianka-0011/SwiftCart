using System;
using AutoMapper;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Reviews.Dto;

namespace SwiftCart.Application.Reviews.Queries;

public class GetProductReviews(Guid id) : IRequest<List<ReviewDto>>
{
    public Guid Id { get; } = id;
    public class Handler(IReviewRepository repo, IMapper mapper) : IRequestHandler<GetProductReviews, List<ReviewDto>>
    {
        private readonly IReviewRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        public async Task<List<ReviewDto>> Handle(GetProductReviews request, CancellationToken cancellationToken)
        {
            var reviews = await _repo.GetReviewsByProductIdAsync(request.Id);
            return _mapper.Map<List<ReviewDto>>(reviews);
        }
    }

}
