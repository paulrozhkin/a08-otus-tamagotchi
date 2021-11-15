namespace Geocoding.API.Config
{
    public class GeocodingOptions
    {
        public const string Geocoding = nameof(Geocoding);

        public bool UseGeocoding { get; set; } = true;

        public string GoogleApiKey { get; set; }

        public string Language { get; set; } = "ru";

        public string RedisCache { get; set; }
    }
}