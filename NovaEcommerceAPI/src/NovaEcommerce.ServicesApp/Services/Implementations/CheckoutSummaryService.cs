using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary;
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
    public class CheckoutSummaryService : ICheckoutSummaryService
    {
        private readonly ICheckoutSummaryRepository _repository;

        public CheckoutSummaryService(ICheckoutSummaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CheckoutSummaryResponseDto>> GetSummaryAsync(int checkoutSessionId)
        {
            var checkout = await _repository.GetCheckoutSessionAsync(checkoutSessionId);

            if (checkout == null)
            {
                return ApiResponse<CheckoutSummaryResponseDto>.FailResponse("Checkout session not found.", 404);
            }

            if (checkout.UserId == null)
            {
                return ApiResponse<CheckoutSummaryResponseDto>.FailResponse("User not found.", 404);
            }

            var cart = await _repository.GetCartWithItemsAsync(checkout.UserId.Value);

            if (cart == null)
            {
                return ApiResponse<CheckoutSummaryResponseDto>.FailResponse("Cart not found.", 404);
            }

            var items = new List<CheckoutSummaryItemDto>();
            decimal subtotal = 0;

            foreach (var item in cart.Items.Where(x=>!x.IsSavedForLater))
            {
                var image = item.ProductVariant.Product.Images.FirstOrDefault(x => x.IsPrimary);

                decimal totalPrice = item.ProductVariant.Price * item.Quantity;
                subtotal += totalPrice;

                items.Add(new CheckoutSummaryItemDto
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductName = item.ProductVariant.Product.Name,
                    ImageUrl = image?.ImageUrl ?? "",
                    Color = item.ProductVariant.Color,
                    Size = item.ProductVariant.Size,
                    UnitPrice = item.ProductVariant.Price,
                    Quantity = item.Quantity,
                    TotalPrice = totalPrice,
                });

            }
            decimal shipping = checkout.ShippingCost;
            decimal discount = 0;
            decimal tax = subtotal * 0.08m;
            decimal total = subtotal + shipping + tax - discount;

            var priceSummary = new PriceSummaryDto
            {
                Subtotal = subtotal,
                Shipping = shipping,
                Tax = tax,
                Discount = discount,
                Total = total
            };

            var shippingSummary = new ShippingSummaryDto
            {
                FullName = checkout.FirstName + " " + checkout.LastName,
                Email = checkout.Email,
                Phone = checkout.Phone,
                AddressLine1 = checkout.AddressLine1,
                AddressLine2 = checkout.AddressLine2,
                City = checkout.City,
                State = checkout.State,
                Country = checkout.Country,
                PostalCode = checkout.PostalCode,
                ShippingMethod = checkout.ShippingMethod.ToString(),
                ShippingCost = checkout.ShippingCost
            };

            var paymentSummary = new PaymentSummaryDto
            {
                PaymentType = checkout.PaymentType.ToString(),
                CartType = checkout.PaymentMethod?.CardType.ToString(),
                LastFourDigits = checkout.PaymentMethod?.LastFourDigits
            };

            var result = new CheckoutSummaryResponseDto
            {
                Shipping = shippingSummary,

                Payment = paymentSummary,

                Items = items,

                Price = priceSummary,
            };

            return ApiResponse<CheckoutSummaryResponseDto>.SuccessResponse(result); 

        }
    }
}
