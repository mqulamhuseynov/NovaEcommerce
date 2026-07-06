namespace NovaEcommerce.Domain.Entities;

public class FlashSale
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsActive { get; set; }

    public ICollection<FlashSaleItem> Items { get; set; } = new List<FlashSaleItem>();
}
