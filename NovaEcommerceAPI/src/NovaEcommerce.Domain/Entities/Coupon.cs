using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}
