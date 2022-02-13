using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models
{
    public class RestaurantRequest
    {
        public string Title { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsParkingPresent { get; set; }

        public bool IsCardPaymentPresent { get; set; }

        public bool IsWiFiPresent { get; set; }

        public List<Guid> Photos { get; set; }
    }
}