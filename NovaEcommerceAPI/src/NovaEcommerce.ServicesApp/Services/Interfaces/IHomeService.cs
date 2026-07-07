using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces
{
    public interface IHomeService
    {
        Task<List<HeroBannerDto>> TGetHeroBannersAsync();
        Task<List<CuratedPickDto>> TGetCuratedPicksAsync();
        Task<List<CategoryDto>> TGetCategoriesAsync();  
    }
}
