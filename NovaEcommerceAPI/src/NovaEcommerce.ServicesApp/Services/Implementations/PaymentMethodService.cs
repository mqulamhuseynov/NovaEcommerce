using NovaEcommerce.DataAccess.Repositories.Interfaces;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.PaymentMethods;
using NovaEcommerce.ServicesApp.Services.Interfaces;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodService(
        IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PaymentMethodResponseDto>>
        GetAllAsync(int userId)
    {
        var methods =
            await _repository.GetAllAsync(userId);

        return methods
            .Select(x => new PaymentMethodResponseDto
            {
                Id = x.Id,
                CardType = x.CardType,
                LastFourDigits = x.LastFourDigits,
                ExpiryMonth = x.ExpiryMonth,
                ExpiryYear = x.ExpiryYear,
                CardholderName = x.CardholderName,
                IsDefault = x.IsDefault
            })
            .ToList();
    }

    public async Task AddAsync(
        int userId,
        CreatePaymentMethodDto dto)
    {
        var methods =
            await _repository.GetAllAsync(userId);

        var paymentMethod =
            new PaymentMethod
            {
                UserId = userId,

                CardType = dto.CardType,

                LastFourDigits =
                    dto.CardNumber.Substring(
                        dto.CardNumber.Length - 4),

                ExpiryMonth = dto.ExpiryMonth,

                ExpiryYear = dto.ExpiryYear,

                CardholderName = dto.CardholderName,

                IsDefault = !methods.Any()
            };

        await _repository.AddAsync(paymentMethod);

        await _repository.SaveChangesAsync();
    }

    public async Task SetDefaultAsync(
        int userId,
        int id)
    {
        var methods =
            await _repository.GetAllAsync(userId);

        foreach (var method in methods)
        {
            method.IsDefault = false;

            await _repository.UpdateAsync(method);
        }

        var selected =
            await _repository.GetByIdAsync(id, userId);

        if (selected == null)
            throw new Exception("Payment method not found.");

        selected.IsDefault = true;

        await _repository.UpdateAsync(selected);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        int userId,
        int id)
    {
        var paymentMethod =
            await _repository.GetByIdAsync(id, userId);

        if (paymentMethod == null)
            throw new Exception("Payment method not found.");

        await _repository.DeleteAsync(paymentMethod);

        await _repository.SaveChangesAsync();
    }
}