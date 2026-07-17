using NovaEcommerce.ServicesApp.DTOs.Address;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IAddressService
    {
        Task<ApiResponse<List<AddressResponseDto>>> GetAllAsync(int userId);

        Task<ApiResponse<string>> CreateAsync(int userId, CreateAddressDto addressDto);

        Task<ApiResponse<string>> SetDefaultAsync(int userId, int addressId);

        Task<ApiResponse<string>> DeleteAsync(int userId, int addressId);
    }
}
