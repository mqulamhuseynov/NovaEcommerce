using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface ICheckoutPaymentService
    {
        Task<ApiResponse<CheckoutPaymentResponseDto>> SavePayment(CheckoutPaymentRequestDto request, int? userId);
    }
}
