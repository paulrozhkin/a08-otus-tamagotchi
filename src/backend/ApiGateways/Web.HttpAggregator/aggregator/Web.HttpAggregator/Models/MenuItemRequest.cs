using System;

namespace Web.HttpAggregator.Models
{
    public class MenuItemRequest
    {
        public Guid DishId { get; set; }

        public long PriceRubles { get; set; }
    }
}