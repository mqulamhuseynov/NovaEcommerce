using NovaEcommerce.ServicesApp.DTOs.Checkout.PlaceOrderConfirmation;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IPlaceOrderService
    {
        Task<ApiResponse<PlaceOrderResponseDto>> PlaceOrderAsync(int userId);
    }
}
