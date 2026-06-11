using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications.AddressSpecifications;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.LocationService
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationService _locationService;

        public AddressService(IUnitOfWork unitOfWork, ILocationService locationService)
        {
            _unitOfWork = unitOfWork;
            _locationService = locationService;
        }

        public async Task<ReturnAddress> SaveAddressAsync(string userId,AddressCreateDto addressDto)
        {

            var GetData = await _locationService.GetAddressAsync(addressDto.Latitude, addressDto.Longitude);

            var data = new Address
            {
                Lable = addressDto.Lable,
                Latitude = addressDto.Latitude,
                Longitude = addressDto.Longitude,
                Details = addressDto.Details,
                Street = GetData?.Street ?? "غير محدد",
                City = GetData?.City ?? "غير محدد",
                AppUserId = userId,
                CreatedDate = DateTime.UtcNow
            };


            await _unitOfWork.Repository<Address>().AddAsync(data);
            await _unitOfWork.CompleteAsync();

            return new ReturnAddress
            {
                Id = data.Id,
                Lable = data.Lable,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                Street = data.Street,
                City = data.City,
                Details = data.Details
            };
        }

        public async Task<ReturnAddress> GetAddressByIdAsync(string userId , int id)
        {
            var spec = new AddresswhereandsortSpecification(userId, id);
            var address = await _unitOfWork.Repository<Address>().GetWithSpecAsync(spec);
            if (address == null) return null;

            return new ReturnAddress
            {
                Id = address.Id,
                Lable = address.Lable,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Street = address.Street,
                City = address.City,
                Details = address.Details
            };
        }

        public async Task<IReadOnlyList<ReturnAddress>> GetAllAddressesAsync(string userId)
        {
            var spec = new AddresswhereandsortSpecification(userId);

            var data = await _unitOfWork.Repository<Address>().GetAllWithSpecAsync(spec);

            return data.Select(a => new ReturnAddress
            {
                Id = a.Id,
                Lable = a.Lable,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                Street = a.Street,
                City = a.City,
                Details = a.Details
            }).ToList();
        }

        public async Task RemoveAddressAsync(int id)
        {
            var address = await _unitOfWork.Repository<Address>().GetAsync(id);
            if (address != null)
            {
                _unitOfWork.Repository<Address>().Delete(address);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
