using System;

namespace Web.HttpAggregator.Models
{
    public class TableResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlaces { get; set; }
    }
}