using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class DishResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Uri> Photos { get; set; }

        public string Description { get; set; }
    }
}
