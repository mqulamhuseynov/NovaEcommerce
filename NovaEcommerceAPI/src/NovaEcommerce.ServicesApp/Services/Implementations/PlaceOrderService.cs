using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.Checkout.PlaceOrderConfirmation;
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
    public class PlaceOrderService : IPlaceOrderService
    {
        private readonly IPlaceOrderRepository _repository;
        public PlaceOrderService(IPlaceOrderRepository repository)
        {
            _repository = repository;
        }


        public async Task<ApiResponse<PlaceOrderResponseDto>> PlaceOrderAsync(int userId)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var checkout = await _repository.GetCheckoutSessionByUserIdAsync(userId);
                if (checkout == null)
                {
                    return ApiResponse<PlaceOrderResponseDto>.FailResponse("Checkout not found.", 404);
                }

                var cart = await _repository.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.Items.Any())
                {
                    return ApiResponse<PlaceOrderResponseDto>.FailResponse("Cart is empty.", 404);
                }

                decimal subtotal = cart.Items
                                         .Sum(x => x.ProductVariant.Price * x.Quantity);

                decimal tax = subtotal * 0.08m;

                decimal discount = 0;

                if (!string.IsNullOrEmpty(cart.AppliedCouponCode))
                {
                    var coupon =
                        await _repository.GetCouponAsync(cart.AppliedCouponCode);

                    if (coupon != null)
                    {
                        if (coupon.DiscountType == DiscountType.Percentage)
                            discount = subtotal * coupon.DiscountValue / 100;

                        else
                            discount = coupon.DiscountValue;
                    }
                }

                decimal total = subtotal - discount + checkout.ShippingCost + tax;

                string orderNumber;
                do
                {
                    orderNumber = "NV-" + Random.Shared.Next(100000, 999999);
                } while (await _repository.OrderNumberExistsAsync(orderNumber));


                DateTime start;
                DateTime end;

                switch (checkout.ShippingMethod)
                {
                    case ShippingMethod.Standard:
                        start = DateTime.UtcNow.AddDays(3);
                        end = DateTime.UtcNow.AddDays(5);
                        break;

                    case ShippingMethod.Express:
                        start = DateTime.UtcNow.AddDays(1);
                        end = DateTime.UtcNow.AddDays(2);
                        break;

                    default:
                        start = DateTime.UtcNow;
                        end = DateTime.UtcNow;
                        break;
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderNumber = orderNumber,
                    Status = OrderStatus.Processing,
                    Subtotal = subtotal,
                    ShippingCost = checkout.ShippingCost,
                    Tax = tax,
                    Discount = discount,
                    Total = total,
                    PaymentMethodId = checkout.PaymentMethodId,
                    ShippingAddressSnapshot = $"{checkout.AddressLine1}, {checkout.City}, {checkout.Country}",
                    PaymentCardTypeSnapshot = checkout.PaymentMethod?.CardType.ToString(),
                    PaymentLastFourSnapshot = checkout.PaymentMethod?.LastFourDigits.ToLower(),
                    ShippingMethod = checkout.ShippingMethod.ToString(),
                    PlacedAt = DateTime.UtcNow,
                    EstimatedDeliveryStart = start,
                    EstimatedDeliveryEnd = end,
                };

                await _repository.AddOrderAsync(order);

                var orderItems = new List<OrderItem>();

                foreach (var item in cart.Items.Where(x=>!x.IsSavedForLater))
                {
                    if (item.ProductVariant.StockQuantity < item.Quantity)
                    {
                        return ApiResponse<PlaceOrderResponseDto>.FailResponse($"{item.ProductVariant.Product.Name} is out of stock.", 404);
                    }

                    item.ProductVariant.StockQuantity -= item.Quantity;

                    _repository.UpdateProductVariant(item.ProductVariant);

                    orderItems.Add(new OrderItem
                    {
                        Order = order,
                        ProductVariantId = item.ProductVariantId,
                        ProductNameSnapshot = item.ProductVariant.Product.Name,
                        ColorSnapshot = item.ProductVariant.Color,
                        SizeSnapshot = item.ProductVariant.Size,
                        PriceSnapshot = item.ProductVariant.Price,
                        Quantity = item.Quantity
                    });

                }

                await _repository.AddOrderItemAsync(orderItems);

                var history = new OrderStatusHistory
                {
                    Order = order,
                    Status = OrderStatus.Processing,
                    ChangedAt = DateTime.UtcNow
                };

                await _repository.AddOrderStatusHistoryAsync(history);

                _repository.RemoveCartItems(cart.Items.ToList());

                _repository.RemoveCheckoutSession(checkout);

                await _repository.SaveChangesAsync();

                await _repository.CommitTransactionAsync(); 

                var OrderShipping = new OrderShippingConfirmationDto
                {
                    FullName = checkout.FirstName + " " + checkout.LastName,
                    Address = checkout.AddressLine1,
                    PostalCode = checkout.PostalCode,
                    City = checkout.City,
                    Country = checkout.Country
                };

                var OrderPrice = new OrderPriceConfirmationDto
                {
                    Subtotal = order.Subtotal,
                    Shipping = order.ShippingCost,
                    Tax = order.Tax,
                    Total = order.Total
                };

                var OrderItemsresponse = orderItems.Select(x => new OrderItemConfirmationDto
                {
                    ProductName = x.ProductNameSnapshot,
                    ImageUrl = x.ProductVariant.Product.Images.FirstOrDefault(x => x.IsPrimary)?.ImageUrl ?? "",
                    Color = x.ColorSnapshot,
                    Size = x.SizeSnapshot,
                    Price = x.PriceSnapshot,
                    Quantity = x.Quantity
                }).ToList();



                var response = new PlaceOrderResponseDto
                {
                    OrderNumber = order.OrderNumber,

                    EstimatedDeliveryStart = order.EstimatedDeliveryStart!.Value,

                    EstimatedDeliveryEnd = order.EstimatedDeliveryEnd!.Value,

                    Shipping = OrderShipping,

                    PriceConfirmation = OrderPrice,

                    Items = OrderItemsresponse
                };

                return ApiResponse<PlaceOrderResponseDto>.SuccessResponse(response);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }

        }
    }
}
