using System;
using System.Linq;
using System.Threading.Tasks;
using Geocoding.API.Config;
using Geocoding.API.Models;
using Geocoding.Google;
using Microsoft.Extensions.Options;

namespace Geocoding.API.Services.Geocoding
{
    public class GoogleGeocoding : IGeocoding
    {
        private readonly IGeocoder _geocoder;

        public GoogleGeocoding(IOptions<GeocodingOptions> googleMapConfig)
        {
            if (string.IsNullOrWhiteSpace(googleMapConfig.Value.GoogleApiKey))
            {
                throw new ArgumentNullException(nameof(googleMapConfig.Value.GoogleApiKey));
            }

            _geocoder = new GoogleGeocoder()
            {
                ApiKey = googleMapConfig.Value.GoogleApiKey, Language = googleMapConfig.Value.Language
            };
        }

        public async Task<GeocodeInfo> GeocodeAsync(string address)
        {
            var addresses = await _geocoder.GeocodeAsync(address);
            var firstAddress = addresses.FirstOrDefault();

            if (firstAddress == null)
            {
                return null;
            }

            return new GeocodeInfo()
            {
                FormattedAddress = firstAddress.FormattedAddress,
                Latitude = firstAddress.Coordinates.Latitude,
                Longitude = firstAddress.Coordinates.Longitude
            };
        }

        public async Task<string> ReverseGeocodeAsync(double latitude, double longitude)
        {
            var addresses = await _geocoder.ReverseGeocodeAsync(latitude, longitude);
            var firstAddress = addresses.FirstOrDefault();

            return firstAddress?.FormattedAddress;
        }
    }
}