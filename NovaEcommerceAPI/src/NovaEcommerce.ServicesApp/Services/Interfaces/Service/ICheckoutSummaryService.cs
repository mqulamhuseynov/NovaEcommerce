using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface ICheckoutSummaryService
    {
        Task<ApiResponse<CheckoutSummaryResponseDto>> GetSummaryAsync(int checkoutSessionId);
    }
}
