namespace FarmFresh.ViewModels.Review;

public record ProductReviewDto
{
    public string Content { get; set; } = string.Empty;

    public int Rating { get; set; }

    public DateTime ReviewDate { get; set; }

    public Guid ProductId { get; set; }

    public string UserFullName { get; set; } = string.Empty;
}
