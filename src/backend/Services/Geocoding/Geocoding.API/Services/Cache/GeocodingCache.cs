using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Geocoding.API.Services.Cache
{
    public class GeocodingCache : IGeocodingCache
    {
        private readonly IDistributedCache _cache;

        public GeocodingCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public Task<string> GetLocationFromCache(double latitude, double longitude)
        {
            return _cache.GetStringAsync($"location:{latitude};{longitude}");
        }

        public async Task AddLocationToCache(double latitude, double longitude, string formattedAddress)
        {
            var options = new DistributedCacheEntryOptions();
            await _cache.SetStringAsync($"location:{latitude};{longitude}", formattedAddress, options);
        }
    }
}