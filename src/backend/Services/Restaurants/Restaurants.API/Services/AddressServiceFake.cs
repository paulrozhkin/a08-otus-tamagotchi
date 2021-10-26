using System.Threading.Tasks;
using Restaurants.Domain.Services;

namespace Restaurants.API.Services
{
    public class AddressServiceFake : IAddressService
    {
        public Task<string> GetAddressFromLocation(double latitude, double longitude)
        {
            return Task.FromResult($"Address for location: latitude - {latitude} and longitude - {longitude}");
        }
    }
}