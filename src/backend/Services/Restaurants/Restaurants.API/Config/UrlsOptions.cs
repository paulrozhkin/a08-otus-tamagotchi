namespace Restaurants.API.Config
{
    public class GeocodingOptions
    {
        public const string Geocoding = "Geocoding";

        public bool UseGeocoding { get; set; }

        public string GeocodingGrpc { get; set; }
    }
}
