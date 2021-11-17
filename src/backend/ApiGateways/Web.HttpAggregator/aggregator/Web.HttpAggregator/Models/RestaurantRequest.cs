namespace Web.HttpAggregator.Models
{
    public class RestaurantRequest
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsParkingPresent { get; set; }

        public bool IsCardPaymentPresent { get; set; }

        public bool IsWiFiPresent { get; set; }
    }
}