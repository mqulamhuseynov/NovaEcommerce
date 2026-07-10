using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.FlashSaleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IFlashSaleRepository
    {
        Task<FlashSale?> GetActiveAsync();
        Task<List<FlashSale>> GetUpComingAsync();   
        Task<FlashSale?> GetByIdAsync(int id);
    }
}
