using NovaEcommerce.ServicesApp.Services;
using NovaEcommerce.ServicesApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.DTOs.Responses;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _repository;

        public HomeService(IHomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDto>> TGetCategoriesAsync()
        {
            var data = await _repository.GetCategoriesAsync();

            return data.Select(x => new CategoryDto
            { 
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,  
                ImageUrl = x.ImageUrl,
                IconUrl = x.IconUrl
            }).ToList();

        }

        public async Task<ApiResponse<List<CuratedPickDto>>> TGetCuratedPicksAsync()
        {
            var data = await _repository.GetCuratedPicksAsync();

            var dto = data.Select(x => new CuratedPickDto
            {
                Id = x.Id,
                Name = x.Name,
                Brand = x.Brand.Name,

                Price = x.Variants
                    .Where(v => v.IsActive)
                    .OrderBy(v => v.Price)
                    .Select(v => v.Price)
                    .FirstOrDefault(),

                ImageUrl = x.Images
                    .OrderBy(i => i.DisplayOrder)
                    .FirstOrDefault(i => i.IsPrimary)?.ImageUrl

            }).ToList();

            return ApiResponse<List<CuratedPickDto>>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<List<HeroBannerDto>>> TGetHeroBannersAsync()
        {
            var data = await _repository.GetHeroBannersAsync();

            var dto = data.Select(x => new HeroBannerDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                LinkUrl = x.LinkUrl
            }).ToList();

            return ApiResponse<List<HeroBannerDto>>.SuccessResponse(dto);

        }
    }
}
