using NovaEcommerce.ServicesApp.DTOs.Product;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IProductService
    {
        public Task<ApiResponse<ProductDetailDto>> GetProductDetail(int id);
        public Task<ApiResponse<PaginationResponse<ProductListDto>>> GetPLP(ProductFilterDto filter);
    }
}
