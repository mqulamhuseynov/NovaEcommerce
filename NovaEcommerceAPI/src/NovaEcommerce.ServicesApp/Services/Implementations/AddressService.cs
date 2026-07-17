using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.Address;
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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<string>> CreateAsync(int userId, CreateAddressDto addressDto)
        {
            var hasAddress = await _repository.AnyAddressAsync(userId);

            var address = new Address()
            {
                UserId = userId,
                Label = addressDto.Label,
                FirstName = addressDto.FirstName,
                LastName = addressDto.LastName,
                AddressLine1 = addressDto.AddressLine1,
                AddressLine2 = addressDto.AddressLine2,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                Phone = addressDto.Phone,
                IsDefault = !hasAddress
            };

            await _repository.AddAsync(address);
            await _repository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Address created successfully.");
        }
        
        public async Task<ApiResponse<List<AddressResponseDto>>> GetAllAsync(int userId)
        {
            var addresses = await _repository.GetUserAddressesAsync(userId);

            if(addresses == null)
            {
                return ApiResponse<List<AddressResponseDto>>.FailResponse("Address not found", 404);
            }
            
            var response = addresses.Select(x=> new AddressResponseDto
            {
                Id = x.Id,
                Label = x.Label,
                FullName = x.FirstName + " " + x.LastName,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                City = x.City,
                State = x.State,
                PostalCode = x.PostalCode,
                Country = x.Country,
                Phone= x.Phone,
                IsDefault = x.IsDefault,    
            }).ToList();   
            
            return ApiResponse<List<AddressResponseDto>>.SuccessResponse(response);    
        }

        public async Task<ApiResponse<string>> DeleteAsync(int userId, int addressId)
        {
            var address = await _repository.GetByIdAsync(addressId, userId);

            if(address == null)
            {
                return ApiResponse<string>.FailResponse("Address not found", 404);
            }

            _repository.DeleteAddress(address);
            await _repository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Address deleted successfully.");
        }

        public async Task<ApiResponse<string>> SetDefaultAsync(int userId, int addressId)
        {
            var address = await _repository.GetByIdAsync(addressId, userId);

            if (address == null)
            {
                return ApiResponse<string>.FailResponse("Address not found", 404);
            }

            await _repository.SetAllNonDefaultAsync(userId);

            address.IsDefault = true;

            await _repository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Choose default address.");


        }
    }
}
