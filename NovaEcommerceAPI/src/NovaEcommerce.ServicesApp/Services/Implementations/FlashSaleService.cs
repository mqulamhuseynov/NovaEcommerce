using Microsoft.EntityFrameworkCore.Storage.Json;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.FlashSaleDtos;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class FlashSaleService : IFlashSaleService
    {
        private readonly IFlashSaleRepository _flashSaleRepo;
        private readonly IFlashSaleNotifyRepository _flashSaleNotifyRepo;

        public FlashSaleService(IFlashSaleRepository flashSaleRepo, IFlashSaleNotifyRepository flashSaleNotifyRepo)
        {
            _flashSaleRepo = flashSaleRepo;
            _flashSaleNotifyRepo = flashSaleNotifyRepo;
        }

        public async Task<ApiResponse<ActiveFlashSaleDto>> TGetActiveAsync()
        {
            var sale = await _flashSaleRepo.GetActiveAsync();

            if(sale == null)
            {
                return new ApiResponse<ActiveFlashSaleDto>
                (
                    false,
                    "No active flash sale found.",
                    null!
                );
            }

            var result = new ActiveFlashSaleDto
            {
                Id = sale.Id,
                Name = sale.Name,
                EndsAt = sale.EndsAt,

                Items = sale.Items.Select(x => new FlashSaleItemDto
                {
                    Id = x.Id,
                    ProductVariantId = x.ProductVariantId,
                    ProductName = x.ProductVariant.Product.Name,
                    ImageUrl = x.ProductVariant.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    Price = x.ProductVariant.Price,
                    OriginalPrice = x.ProductVariant.OriginalPrice ?? 0,
                    DiscountPercentage = x.DiscountPercentage,
                    SoldCount = x.SoldCount,
                    StockLimit = x.StockLimit

                }).ToList()

            };

            var response = new ApiResponse<ActiveFlashSaleDto>
                (
                    true,
                    "Active flash sale retrieved successfully.",
                    result
                );

            return response;

        }

        public async Task<ApiResponse<List<UpcomingFlashSaleDto>>> TGetUpcomingAsync()
        {
            var sale = await _flashSaleRepo.GetUpComingAsync();

            if(sale == null)
            {
                return new ApiResponse<List<UpcomingFlashSaleDto>>
                    (
                        false,
                        "No upcoming flash sales found.",
                        null!
                    );
            }

            var result = sale.Select(s => new UpcomingFlashSaleDto
            {
                Id = s.Id,

                Name = s.Name,

                StartsAt = s.StartsAt,

                EndsAt = s.EndsAt,

                Items = s.Items.Select (x=> new FlashSaleItemDto
                {

                    Id = x.Id,
                    ProductVariantId = x.ProductVariantId,
                    ProductName = x.ProductVariant.Product.Name,
                    ImageUrl = x.ProductVariant.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    Price = x.ProductVariant.Price,
                    OriginalPrice = x.ProductVariant.OriginalPrice ?? 0,
                    DiscountPercentage = x.DiscountPercentage,
                    SoldCount = x.SoldCount,
                    StockLimit = x.StockLimit

                }).ToList()

            }).ToList();

            var response = new ApiResponse<List<UpcomingFlashSaleDto>>
                (
                    true,
                    "Upcoming flash sales retrieved successfully.",
                    result
                );

            return response;    

        }

        public async Task<ApiResponse<string>> TNotifyAsync(int flashSaleId, string email)
        {
            var flashSale = await _flashSaleRepo.GetByIdAsync(flashSaleId);

            if(flashSale == null)
            {
                throw new KeyNotFoundException("Flash sale not found.");
            }

            var exist = await _flashSaleNotifyRepo.ExistsAsync(flashSaleId, email);

            if(exist)
            {
                throw new InvalidOperationException("You have already requested notification.");
            }

            var notify = new NotifyRequest
            {
                FlashSaleId = flashSaleId,
                Email = email
            };

            await _flashSaleNotifyRepo.AddAsync(notify);
            await _flashSaleNotifyRepo.SaveChangesAsync();

            var response = new ApiResponse<string>
                (
                    true,
                    "Notification request created successfully.",
                    null!
                );

            return response;
        }
    }
}
