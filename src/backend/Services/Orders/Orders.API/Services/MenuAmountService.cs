using System.Collections.Generic;
using System.Threading.Tasks;
using MenuApi;
using Orders.Domain.Models;
using Orders.Domain.Services;

namespace Orders.API.Services
{
    public class MenuAmountService : IMenuAmountService
    {
        private readonly Menu.MenuClient _menuClient;

        public MenuAmountService(Menu.MenuClient menuClient)
        {
            _menuClient = menuClient;
        }

        public async Task<int> CalculateAmountForMenuPositions(List<MenuPosition> positions)
        {
            var amount = 0;
            foreach (var position in positions)
            {
                var menuItem = await _menuClient.GetMenuItemAsync(new GetMenuItemRequest()
                {
                    Id = position.MenuItemId.ToString()
                });

                amount += (int)menuItem.MenuItem.PriceRubles * position.Count;
            }

            return amount;
        }
    }
}
