using System.Threading.Tasks;
using Geocoding.API.Services.Cache;
using Geocoding.API.Services.Geocoding;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Geocoding.API
{
    public class GeocodingService : Geocoding.GeocodingBase
    {
        private readonly ILogger<GeocodingService> _logger;
        private readonly IGeocoding _geocoding;
        private readonly IGeocodingCache _geocodingCache;

        public GeocodingService(ILogger<GeocodingService> logger,
            IGeocoding geocoding,
            IGeocodingCache geocodingCache)
        {
            _logger = logger;
            _geocoding = geocoding;
            _geocodingCache = geocodingCache;
        }

        public override async Task<GeocodeResponse> Geocode(GeocodeRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
            { 
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Address null or empty"));
            }
            
            var result = await _geocoding.GeocodeAsync(request.Address);

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Not found information about address"));
            }

            return new GeocodeResponse()
            {
                FormattedAddress = result.FormattedAddress,
                Latitude = result.Latitude,
                Longitude = result.Longitude
            };
        }

        public override async Task<ReverseGeocodeResponse> ReverseGeocode(ReverseGeocodeRequest request, ServerCallContext context)
        {
            if (request.Longitude == 0 || request.Latitude == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Longitude and Latitude can't be 0"));
            }

            var cachedAddress = await _geocodingCache.GetLocationFromCache(request.Latitude, request.Longitude);
            var result = cachedAddress;

            if (string.IsNullOrWhiteSpace(result))
            {
                result = await _geocoding.ReverseGeocodeAsync(request.Latitude, request.Longitude);
                await _geocodingCache.AddLocationToCache(request.Latitude, request.Longitude, result);
            }

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Not found information about address"));

            }

            return new ReverseGeocodeResponse()
            {
                FormattedAddress = result
            };
        }
    }
}
