using System.Text.Json.Serialization;

namespace Web.HttpAggregator.Models;

public class NextStatusResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus OrderStatus { get; set; }
}