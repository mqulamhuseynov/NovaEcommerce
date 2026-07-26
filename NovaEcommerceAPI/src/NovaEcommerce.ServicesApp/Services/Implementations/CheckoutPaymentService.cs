using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class CheckoutPaymentService : ICheckoutPaymentService
    {
        private readonly ICheckoutPaymentRepository _repository;

        public CheckoutPaymentService(ICheckoutPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CheckoutPaymentResponseDto>> SavePayment(CheckoutPaymentRequestDto request, int? userId)
        {
            var checkout = await _repository.GetCheckoutSessionAsync(request.CheckoutSessionId);

            if(checkout == null)
            {
                return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Checkout session not found.", 404);
            }

            checkout.PaymentType = (PaymentType)request.PaymentType;

            string? lastFour = null;

            if ((PaymentType)request.PaymentType == PaymentType.Card)
            {
                if (string.IsNullOrWhiteSpace(request.CardNumber))
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Card number is required for card payments.", 400);
                }
                if (request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Card number must contain 16 digits.", 400);
                }

                if (string.IsNullOrWhiteSpace(request.CVV))
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("CVV is required.", 400);
                }

                if ((request.CVV.Length < 3 || request.CVV.Length > 4) || !request.CVV.All(char.IsDigit))
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Invalid CVV.", 400);
                }

                if (request.ExpiryYear==null || request.ExpiryMonth==null)
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Expiry date is required.", 400);
                }

                var expiry = new DateTime(request.ExpiryYear.Value, request.ExpiryMonth.Value, 1).AddMonths(1).AddDays(-1);

                if(expiry<DateTime.UtcNow)
                {
                    return ApiResponse<CheckoutPaymentResponseDto>.FailResponse("Card has expired.", 400);
                }

                CardType cartType = request.CardNumber.StartsWith("4") ? CardType.Visa : CardType.Mastercard;

                lastFour = request.CardNumber.Substring(request.CardNumber.Length - 4);

                var paymentMethod = new PaymentMethod
                {
                    UserId = userId ?? 0,
                    CardType = cartType,
                    LastFourDigits = lastFour,
                    ExpiryMonth = request.ExpiryMonth.Value,
                    ExpiryYear = request.ExpiryYear.Value,
                    CardholderName = request.CardHolderName!,
                    IsDefault = true
                };

                await _repository.AddPaymentMethodAsync(paymentMethod);

                checkout.PaymentMethod = paymentMethod;
            }

            await _repository.SaveChangesAsync();

            var response = new CheckoutPaymentResponseDto
            {
                PaymentMethodId = checkout.PaymentMethod?.Id ?? 0,
                PaymentType = checkout.PaymentType.ToString(),
                LastFourDigits = lastFour
            };

            return ApiResponse<CheckoutPaymentResponseDto>.SuccessResponse
                (
                    response,
                    "Payment information saved successfully."
                );
        }
    }
}
