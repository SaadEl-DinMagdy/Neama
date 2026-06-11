using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface ILocationService
    {
        Task<AddressResultDto> GetAddressAsync(double lat, double lon);
    }
}
