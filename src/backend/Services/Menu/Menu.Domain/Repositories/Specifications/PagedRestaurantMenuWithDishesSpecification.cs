using System;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Repositories.Specifications
{
    public sealed class PagedRestaurantMenuWithDishesSpecification : BaseSpecification<MenuItem>
    {
        public PagedRestaurantMenuWithDishesSpecification(Guid restaurantId, int pageNumber, int pageSize)
            : base(x =>
                x.RestaurantId == restaurantId)
        {
            ApplyOrderBy(x => x.CreatedDate);
            ApplyPaging(pageNumber, pageSize);
            AddInclude(x => x.Dish);
        }
    }
}