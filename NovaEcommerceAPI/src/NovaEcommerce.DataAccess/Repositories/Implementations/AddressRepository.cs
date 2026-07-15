using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
        }

        public async Task<bool> AnyAddressAsync(int userId)
        {
            return await _context.Addresses.AnyAsync(x=>x.UserId == userId);
        }

        public void DeleteAddress(Address address)
        {
            _context.Addresses.Remove(address);
        }

        public async Task<Address?> GetByIdAsync(int id, int userId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x=> x.Id==id && x.UserId==userId);
        }

        public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            return await _context.Addresses.Where(x=>x.UserId==userId).OrderByDescending(x=>x.IsDefault).ToListAsync();  
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SetAllNonDefaultAsync(int userId)
        {
            var addresses = await _context.Addresses.Where(x => x.UserId==userId).ToListAsync();  

            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }
        }
    }
}
