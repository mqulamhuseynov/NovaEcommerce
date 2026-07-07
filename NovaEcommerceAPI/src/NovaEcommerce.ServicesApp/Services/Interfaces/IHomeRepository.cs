using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Interfaces
{
    public interface IHomeRepository
    {
        Task<List<HeroBanner>> GetHeroBannersAsync();

        Task<List<Product>> GetCuratedPicksAsync();

        Task<List<Category>> GetCategoriesAsync();
    }
}
