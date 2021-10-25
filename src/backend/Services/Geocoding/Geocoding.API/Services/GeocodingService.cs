using System.Threading.Tasks;
using Geocoding.API.Services.Geocoding;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Geocoding.API
{
    public class GeocodingService : Geocoding.GeocodingBase
    {
        private readonly ILogger<GeocodingService> _logger;
        private readonly IGeocoding _geocoding;

        public GeocodingService(ILogger<GeocodingService> logger, IGeocoding geocoding)
        {
            _logger = logger;
            _geocoding = geocoding;
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

            var result = await _geocoding.ReverseGeocodeAsync(request.Latitude, request.Longitude);

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
