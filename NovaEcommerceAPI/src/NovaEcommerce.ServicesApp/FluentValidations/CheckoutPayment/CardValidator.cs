using FluentValidation;
using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.FluentValidations.CheckoutPayment
{
    public class CardValidator : AbstractValidator<CheckoutPaymentRequestDto>
    {
        public CardValidator()
        {
            RuleFor(x=>x.CardNumber)
                .NotEmpty()
                .Length(16)
                .Matches(@"^\d{16}$");


            RuleFor(x => x.CVV)
                .NotEmpty()
                .Matches(@"^\d{3,4}$");

        }
    }
}
