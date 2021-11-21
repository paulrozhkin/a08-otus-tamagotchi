using System.Threading.Tasks;
using Geocoding.API.Models;

namespace Geocoding.API.Services.Geocoding
{
    public class GeocodingFake : IGeocoding
    {
        public Task<GeocodeInfo> GeocodeAsync(string address)
        {
            return Task.FromResult(new GeocodeInfo()
            {
                FormattedAddress = $"Fake location for address: {address}",
                Latitude = 5,
                Longitude = 5
            });
        }

        public Task<string> ReverseGeocodeAsync(double latitude, double longitude)
        {
            return Task.FromResult($"Fake address for location: latitude - {latitude} and longitude - {longitude}");
        }
    }
}