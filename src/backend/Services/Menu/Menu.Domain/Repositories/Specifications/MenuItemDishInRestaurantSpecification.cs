using System;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Repositories.Specifications
{
    public class MenuItemDishInRestaurantSpecification : BaseSpecification<MenuItem>
    {
        public MenuItemDishInRestaurantSpecification(Guid dishId, Guid restaurantId) : base(menuItem =>
            menuItem.DishId == dishId && menuItem.RestaurantId == restaurantId)
        {
        }
    }
}