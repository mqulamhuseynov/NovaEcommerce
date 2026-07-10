using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.Checkout;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class CheckoutShippingService : ICheckoutShippingService
    {
        private readonly ICheckoutShippingRepository _repository;

        public CheckoutShippingService(ICheckoutShippingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CheckoutShippingResponseDto>> SaveShippingAsync(CheckoutShippingRequestDto request, int? userId, string? sessionId)
        {
            if(!new EmailAddressAttribute().IsValid(request.Email))
            {
                return ApiResponse<CheckoutShippingResponseDto>.FailResponse("Invalid email address", 400);
            }

            decimal shippingCost = request.ShippingMethod switch
                                                     {
                                                         ShippingMethod.Standard => 0,
                                                         ShippingMethod.Express => 9.99m,
                                                         ShippingMethod.SameDay => 14.99m,
                                                         _ => throw new Exception("Invalid shipping method")
                                                     };

            var checkout = new CheckoutSession
            {
                UserId = userId,
                SessionId = sessionId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Phone = request.Phone,
                ShippingMethod = request.ShippingMethod,
                ShippingCost = shippingCost,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await _repository.AddAsync(checkout);
            await _repository.SaveChangesAsync();


            var response = new CheckoutShippingResponseDto
            {
                CheckoutSessionId = checkout.Id,
                ShippingMethod = checkout.ShippingMethod.ToString(),
                ShippingCost = checkout.ShippingCost
            };

            return ApiResponse<CheckoutShippingResponseDto>.SuccessResponse(response, "Shipping information saved successfully");

        }
    }
}
