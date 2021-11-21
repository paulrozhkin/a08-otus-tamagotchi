using System;

namespace Web.HttpAggregator.Models
{
    public class MenuItemResponse
    {
        public Guid Id { get; set; }

        public DishResponse Dish { get; set; }

        public long PriceRubles { get; set; }
    }
}