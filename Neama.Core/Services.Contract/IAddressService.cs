using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IAddressService
    {
        Task<ReturnAddress> GetAddressByIdAsync(string userId,int id);

        Task<IReadOnlyList<ReturnAddress>> GetAllAddressesAsync(string userId);
        Task<ReturnAddress> SaveAddressAsync(string userId,AddressCreateDto addressDto);
        Task RemoveAddressAsync(int id);
    }
}
