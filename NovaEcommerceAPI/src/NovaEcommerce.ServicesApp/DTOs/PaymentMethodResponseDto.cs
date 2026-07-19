using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.ServicesApp.DTOs.PaymentMethods;

public class PaymentMethodResponseDto
{
    public int Id { get; set; }

    public CardType CardType { get; set; }

    public string LastFourDigits { get; set; } = default!;

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string CardholderName { get; set; } = default!;

    public bool IsDefault { get; set; }
}