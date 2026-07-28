using FluentValidation;
using NovaEcommerce.ServicesApp.DTOs.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NovaEcommerce.ServicesApp.FluentValidations.PaymentMethod
{
    public class PaymentMethodValidator : AbstractValidator<CreatePaymentMethodDto>
    {
        public PaymentMethodValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card Number is required.")
                .Matches(@"^\d{16}$").WithMessage("Card number must contain exactly 16 digits.");

            RuleFor(x => x.CardholderName)
                .NotEmpty().WithMessage("Cardholder name is required.");

            RuleFor(x => x.ExpiryMonth)
                .InclusiveBetween(1, 12)
                .WithMessage("Expiry month must be between 1 and 12.");

            RuleFor(x => x.ExpiryYear)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("Expiry year cannot be in the past.");
        }
    }
}
