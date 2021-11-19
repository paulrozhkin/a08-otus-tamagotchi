using System;

namespace Web.HttpAggregator.Models
{
    public class MenuItemResponse
    {
        public Guid Id { get; set; }

        public Guid DishId { get; set; }

        public long PriceRubles { get; set; }
    }
}