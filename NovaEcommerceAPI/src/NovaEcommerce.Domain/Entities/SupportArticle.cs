namespace NovaEcommerce.Domain.Entities;

public class SupportArticle
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string Category { get; set; } = default!;
}
