using System;
using Domain.Core.Models;

namespace Menu.Domain.Models
{
    public class MenuItem : BaseEntity
    {
        public Guid RestaurantId { get; set; }

        public Guid DishId { get; set; }

        public Dish Dish { get; set; }

        public long PriceRubles { get; set; }
    }
}