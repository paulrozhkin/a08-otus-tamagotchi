using System.Threading.Tasks;

namespace Restaurants.Domain.Services
{
    public interface IAddressService
    {
        public Task<string> GetAddressFromLocation(double latitude, double longitude);
    }
}
