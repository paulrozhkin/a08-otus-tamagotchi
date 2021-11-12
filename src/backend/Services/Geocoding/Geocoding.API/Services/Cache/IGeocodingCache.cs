using System.Threading.Tasks;

namespace Geocoding.API.Services.Cache
{
    public interface IGeocodingCache
    {
        Task<string> GetLocationFromCache(double latitude, double longitude);

        Task AddLocationToCache(double latitude, double longitude, string formattedAddress);
    }
}
