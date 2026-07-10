
using NovaEcommerce.ServicesApp.DTOs.Checkout;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface ICheckoutShippingService
    {
        Task<ApiResponse<CheckoutShippingResponseDto>> SaveShippingAsync(CheckoutShippingRequestDto request, int? userId, string? sessionId);   
    }
}
