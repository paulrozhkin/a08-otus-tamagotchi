using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class OrderRequest
    {
        public Guid RestaurantId { get; set; }

        public List<OrderPositionRequest> Menu { get; set; }

        public int NumberOfPersons { get; set; }

        public DateTime VisitTime { get; set; }

        public string Comment { get; set; }
    }
}