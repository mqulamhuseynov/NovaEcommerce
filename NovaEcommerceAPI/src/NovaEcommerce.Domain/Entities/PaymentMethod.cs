using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class PaymentMethod
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public CardType CardType { get; set; }
    public string LastFourDigits { get; set; } = default!;
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string CardholderName { get; set; } = default!;
    public bool IsDefault { get; set; }

    public AppUser User { get; set; } = default!;
}
