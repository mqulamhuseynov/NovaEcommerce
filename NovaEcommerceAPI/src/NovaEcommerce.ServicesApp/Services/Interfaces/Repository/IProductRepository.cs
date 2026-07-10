using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IProductRepository
    {
        public IQueryable<Product> GetProductQuery();
        public Task<Product?> GetProductDetails(int id);
        public Task<IEnumerable<Product>> GetRelatedProducts(int categoryId, int currentProductId, int limit);
    }
}
