using Microsoft.AspNetCore.Http;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.ReturnRequest;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IReturnRequestRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReturnRequestService(IReturnRequestRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> CreateAsync(int userId, CreateRequestReturnDto dto)
        {
            var orderItem = await _repository.GetOrderItemForUserAsync(userId, dto.OrderItemId);

            if (orderItem == null)
            {
                return ApiResponse<string>.FailResponse("Order Item not found or you don't have access.", 404);
            }

            if ((dto.ResolutionType == ResolutionType.Exchange) && string.IsNullOrWhiteSpace(dto.ExchangeSize))
            {
                return ApiResponse<string>.FailResponse("Choose new size for Exchange", 404);
            }

            var returnRequest = new ReturnRequest
            {
                OrderItemId = orderItem.Id,
                Reason = dto.Reason,
                ResolutionType = dto.ResolutionType,
                ExchangeSize = dto.ExchangeSize,
                AdditionalNotes = dto.AdditionalNotes,
                Photos = dto.Photos,
                Status = ReturnStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(returnRequest);
            await _repository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Create Return Request");
        }

        public async Task<ApiResponse<List<ReturnRequestDto>>> GetAllAsync(int userId)
        {
            var values = await _repository.GetUserReturnRequestAsync(userId);
            if (values == null || !values.Any())
            {
                return ApiResponse<List<ReturnRequestDto>>.FailResponse("Return not found", 404);
            }

            var result = values.Select(x=> new  ReturnRequestDto
            {
                Id = x.Id,
                ProductName = x.OrderItem.ProductNameSnapshot,
                Color = x.OrderItem.ColorSnapshot,
                Size = x.OrderItem.SizeSnapshot,
                Price = x.OrderItem.PriceSnapshot,  
                Quantity = x.OrderItem.Quantity,
                Reason = x.Reason,
                ResolutionType = x.ResolutionType,  
                ExchangeSize = x.ExchangeSize,  
                AdditionalNotes = x.AdditionalNotes,
                Photos = x.Photos,
                Status = x.Status,  
                CreatedAt = x.CreatedAt,
            }).ToList();    

            return ApiResponse<List<ReturnRequestDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<ReturnRequestDto>> GetByIdAsync(int id, int userId)
        {
            var value = await _repository.GetUserReturnRequestByIdAsync(id, userId);
            if(value == null)
            {
                return ApiResponse<ReturnRequestDto>.FailResponse("Return not found", 404);
            }

            var result = new ReturnRequestDto
            {
                Id = value.Id,
                ProductName = value.OrderItem.ProductNameSnapshot,
                Color = value.OrderItem.ColorSnapshot,
                Size = value.OrderItem.SizeSnapshot,
                Price = value.OrderItem.PriceSnapshot,
                Quantity = value.OrderItem.Quantity,
                Reason = value.Reason,
                ResolutionType = value.ResolutionType,
                ExchangeSize = value.ExchangeSize,
                AdditionalNotes = value.AdditionalNotes,
                Photos = value.Photos,
                Status = value.Status,
                CreatedAt = value.CreatedAt
            };

            return ApiResponse<ReturnRequestDto>.SuccessResponse(result);
        }

        public async Task<ApiResponse<UploadPhotoResponseDto>> UploadPhotoAsync(IFormFile file)
        {
            if (file==null || file.Length==0)
            {
                return ApiResponse<UploadPhotoResponseDto>.FailResponse("File is required", 400);
            }

            var extension = Path.GetExtension(file.FileName);

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            if(!allowedExtensions.Contains(extension.ToLower()))
            {
                return ApiResponse<UploadPhotoResponseDto>.FailResponse("Invalid file type", 400);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            var uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "returns");

            if(!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);    
            }

            var filePath = Path.Combine(uploadFolder, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); 
            }

            var request = _httpContextAccessor.HttpContext?.Request;

            var url = $"{request?.Scheme}://{request?.Host}/uploads/returns/{fileName}";

            var result = new UploadPhotoResponseDto
            {
                Url = url
            };

            return ApiResponse<UploadPhotoResponseDto>.SuccessResponse(result); 
        }
    }
}
