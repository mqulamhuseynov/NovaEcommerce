using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IHomeService
    {
        Task<ApiResponse<List<HeroBannerDto>>> TGetHeroBannersAsync();
        Task<ApiResponse<List<CuratedPickDto>>> TGetCuratedPicksAsync();
        Task<List<CategoryDto>> TGetCategoriesAsync();  
    }
}
