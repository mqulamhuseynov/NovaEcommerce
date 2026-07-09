using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken?> GetByTokenWithUserAsync(string token);
        Task RemoveAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}
