using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.ServicesApp.DTOs.PaymentMethods;

public class CreatePaymentMethodDto
{
    public string CardNumber { get; set; } = default!;

    public CardType CardType { get; set; }

    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string CardholderName { get; set; } = default!;
}