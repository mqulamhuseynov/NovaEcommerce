using Microsoft.AspNetCore.Http;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.ReturnRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IReturnRequestService
    {
        Task<ApiResponse<string>> CreateAsync(int userId, CreateRequestReturnDto dto);

        Task<ApiResponse<List<ReturnRequestDto>>> GetAllAsync(int userId);

        Task<ApiResponse<ReturnRequestDto>> GetByIdAsync(int id, int userId);

        Task<ApiResponse<UploadPhotoResponseDto>> UploadPhotoAsync(IFormFile file);
    }
}
