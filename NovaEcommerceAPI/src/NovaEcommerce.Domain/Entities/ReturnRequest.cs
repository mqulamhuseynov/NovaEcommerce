using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class ReturnRequest
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public string Reason { get; set; } = default!;
    public ResolutionType ResolutionType { get; set; }
    public string? ExchangeSize { get; set; }
    public string? AdditionalNotes { get; set; }
    public List<string> Photos { get; set; } = new();
    public ReturnStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public OrderItem OrderItem { get; set; } = default!;
}
