using NovaEcommerce.ServicesApp.DTOs.FlashSaleDtos;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IFlashSaleService
    {
        Task<ApiResponse<ActiveFlashSaleDto>> TGetActiveAsync();
        Task<ApiResponse<List<UpcomingFlashSaleDto>>> TGetUpcomingAsync();
        Task<ApiResponse<string>> TNotifyAsync(int flashSaleId, string email);
    }
}
