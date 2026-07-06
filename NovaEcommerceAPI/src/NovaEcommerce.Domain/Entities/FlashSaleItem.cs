namespace NovaEcommerce.Domain.Entities;

public class FlashSaleItem
{
    public int Id { get; set; }
    public int FlashSaleId { get; set; }
    public int ProductVariantId { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int SoldCount { get; set; }
    public int StockLimit { get; set; }

    public FlashSale FlashSale { get; set; } = default!;
    public ProductVariant ProductVariant { get; set; } = default!;
}
