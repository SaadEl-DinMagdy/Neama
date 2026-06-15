using Neama.Core.Services.Contract;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;

        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "NeamaProject");
        }

        public async Task<AddressResultDto> GetAddressAsync(double lat, double lon)
        {
            string url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}&addressdetails=1";

            var response = await _httpClient.GetFromJsonAsync<NominatimResponse>(url);

            if (response != null && response.address.road != null)
            {
                return new AddressResultDto
                {
                    Street = response.address.road ?? response.address.suburb,
                    City = response.address.city ?? response.address.town ?? response.address.village,
                    Governorate = response.address.state,
                    DisplayName = response.display_name
                };
            }

            return null;
        }
    }

    
}
