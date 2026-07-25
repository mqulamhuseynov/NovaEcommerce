using FluentValidation;
using NovaEcommerce.ServicesApp.DTOs.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.FluentValidation
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDto> 
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50)
                .Matches(@"^[A-Za-zƏəIıİiÖöĞğÜüÇçŞş]+$")
                .WithMessage("First name can only contain letters.");
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50)
                .Matches(@"^[A-Za-zƏəIıİiÖöĞğÜüÇçŞş]+$")
                .WithMessage("Last name can only contain letters.");
            
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress(); 
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        } 
    }
}
