using NovaEcommerce.ServicesApp.DTOs.PaymentMethods;

namespace NovaEcommerce.ServicesApp.Services.Interfaces;

public interface IPaymentMethodService
{
    Task<List<PaymentMethodResponseDto>> GetAllAsync(int userId);

    Task AddAsync(
        int userId,
        CreatePaymentMethodDto dto);

    Task SetDefaultAsync(
        int userId,
        int id);

    Task DeleteAsync(
        int userId,
        int id);
}