using System;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Repositories.Specifications
{
    public sealed class MenuItemWithDishSpecification : BaseSpecification<MenuItem>
    {
        public MenuItemWithDishSpecification(Guid menuItemId) : base(menuItem =>
            menuItem.Id == menuItemId)
        {
            AddInclude(x => x.Dish);
        }
    }
}