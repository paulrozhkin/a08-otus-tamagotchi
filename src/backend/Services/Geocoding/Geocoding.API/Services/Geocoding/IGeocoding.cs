using System.Collections.Generic;
using System.Threading.Tasks;
using Geocoding.API.Models;

namespace Geocoding.API.Services.Geocoding
{
    public interface IGeocoding
    {
        Task<GeocodeInfo> GeocodeAsync(string address);

        Task<string> ReverseGeocodeAsync(double latitude, double longitude);

    }
}
