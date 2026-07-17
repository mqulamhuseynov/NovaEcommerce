using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetUserAddressesAsync(int userId);
        Task<Address?> GetByIdAsync(int id, int userId);
        Task AddAsync(Address address);
        Task<bool> AnyAddressAsync(int userId);
        Task SetAllNonDefaultAsync(int userId);
        void DeleteAddress(Address address);
        Task SaveChangesAsync();
    }
}
