using System;

namespace SwiftCart.Application.Reviews.Dto;


public class AddReviewDto
{
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class ReviewDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
