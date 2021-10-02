﻿namespace Web.HttpAggregator.Models
{
    public class CreateRestaurantRequest
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsParkingPresent { get; set; }

        public bool IsCardPaymentPresent { get; set; }

        public bool IsWiFiPresent { get; set; }
    }
}
