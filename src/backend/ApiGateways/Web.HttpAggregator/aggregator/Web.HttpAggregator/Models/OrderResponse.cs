using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.HttpAggregator.Models;

public class OrderResponse
{
    public Guid Id { get; set; }

    public RestaurantResponse Restaurant { get; set; }

    public List<OrderPositionResponse> Menu { get; set; }

    public int NumberOfPersons { get; set; }

    public DateTime VisitTime { get; set; }

    public string Comment { get; set; }

    public int AmountRubles { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus OrderStatus { get; set; }

    public DateTime CreatedTime { get; set; }
}

public enum OrderStatus
{
    Created,
    Wait,
    Work,
    Ready,
    Completed,
    Skipped
}