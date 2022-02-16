using System.Collections.Generic;
using System.Threading.Tasks;
using Orders.Domain.Models;
using Orders.Domain.Services;

namespace Orders.API.Services
{
    public class MenuAmountService : IMenuAmountService
    {
        private readonly MenuApi.Menu.MenuClient _menuClient;

        public MenuAmountService(MenuApi.Menu.MenuClient menuClient)
        {
            _menuClient = menuClient;
        }

        //public async Task<string> GetAddressFromLocation(double latitude, double longitude)
        //{
        //    var result = await _geocodingClient.ReverseGeocodeAsync(new ReverseGeocodeRequest()
        //    {
        //        Latitude = latitude,
        //        Longitude = longitude
        //    });

        //    return result.FormattedAddress;
        //}

        public Task<int> CalculateAmountForMenuPositions(List<MenuPosition> positions)
        {
            return Task.FromResult(1);
        }
    }
}
