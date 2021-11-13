using System.Threading.Tasks;
using Geocoding.API;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Services;
using static Geocoding.API.Geocoding;

namespace Restaurants.API.Services
{
    public class AddressService: IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly GeocodingClient _geocodingClient;

        public AddressService(ILogger<AddressService> logger, GeocodingClient geocodingClient)
        {
            _logger = logger;
            _geocodingClient = geocodingClient;
        }

        public async Task<string> GetAddressFromLocation(double latitude, double longitude)
        {
            var result = await _geocodingClient.ReverseGeocodeAsync(new ReverseGeocodeRequest()
            {
                Latitude = latitude,
                Longitude = longitude
            });

            return result.FormattedAddress;
        }
    }
}
