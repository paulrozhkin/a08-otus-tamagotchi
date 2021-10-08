﻿using System.Collections.Generic;

namespace Infrastructure.Core.OrderQueue
{
    public class NewKitchenOrder
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public List<NewOrderDish> Dishes { get; set; }
    }
}