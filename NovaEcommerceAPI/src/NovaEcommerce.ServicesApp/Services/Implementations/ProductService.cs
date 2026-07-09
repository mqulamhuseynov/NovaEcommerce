using Microsoft.EntityFrameworkCore;
using NovaEcommerce.ServicesApp.DTOs.Product;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _product;

        public ProductService(IProductRepository product)
        {
            _product = product;
        }

        public async Task<ApiResponse<PaginationResponse<ProductListDto>>> GetPLP(ProductFilterDto filter)
        {
            var query =  _product.GetProductQuery();

            //kateqori
            if(!string.IsNullOrEmpty(filter.CategorySlug)) query = query.Where(p => p.Category.Slug == filter.CategorySlug && p.IsActive);
            //reng
            if(!string.IsNullOrEmpty(filter.Color)) query = query.Where(p => p.Variants.Any(v => v.Color == filter.Color && p.IsActive));
            //size
            if(!string.IsNullOrEmpty(filter.Size)) query = query.Where(p => p.Variants.Any(v => v.Size == filter.Size && p.IsActive));
            //max qiymet
            if(filter.MaxPrice.HasValue) query = query.Where(p => p.Variants.Any(v => v.Price <= filter.MaxPrice.Value));
            //min qiymet
            if(filter.MinPrice.HasValue) query = query.Where(p => p.Variants.Any(v => v.Price >= filter.MinPrice.Value));

            //sort
            query = filter.Sort.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Variants.Min(v => v.Price)),
                "price_desc" => query.OrderByDescending(p => p.Variants.Max(v => v.Price)),
                "newest" or _  => query.OrderBy(p => p.Name) //newest veya default olaraq ada görə sıralama
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filter.Limit);

            var products = await query
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .ToListAsync();

            var mappedProducts = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                BasePrice = p.BasePrice,
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ?? "",
                ReviewCount = p.Reviews.Count,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                AvailableColors = p.Variants.Where(v => v.IsActive).Select(v => v.Color).Distinct().ToList(),
                AvailableSizes = p.Variants.Where(v => v.IsActive).Select(v => v.Size).Distinct().ToList()
            }).ToList();

            var pagedResponse = new PaginationResponse<ProductListDto>
            {
                Items = mappedProducts,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.Page
            };

            return ApiResponse<PaginationResponse<ProductListDto>>.SuccessResponse(pagedResponse, "Products retrieved successfully", 200);
        }

        public async Task<ApiResponse<ProductDetailDto>> GetProductDetail(int id)
        {
            var product = await _product.GetProductDetails(id);

            if (product == null)
            {
                return ApiResponse<ProductDetailDto>.FailResponse("Product not found", 404);
            }

            var relatedProducts = await _product.GetRelatedProducts(product.CategoryId,product.Id,4);

            var detailDto = new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                ReviewCount = product.Reviews.Count,
                AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                Images = product.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList(),

                Variants = product.Variants.Where(v => v.IsActive).Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Color = v.Color,
                    Size = v.Size,
                    Sku = v.Sku,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity
                }).ToList(),

                RelatedProducts = relatedProducts.Select(r => new RelatedProductDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.BasePrice,
                    ImageUrl = r.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ?? ""
                }).ToList()
            };

            return ApiResponse<ProductDetailDto>.SuccessResponse(detailDto);
        }
    }

}

