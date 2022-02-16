using System;
using System.Collections.Generic;

namespace Web.HttpAggregator.Models;

public class OrderResponse
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public List<OrderPositionResponse> Menu { get; set; }

    public int NumberOfPersons { get; set; }

    public DateTime VisitTime { get; set; }

    public string Comment { get; set; }

    public int AmountRubles { get; set; }

    public OrderStatus OrderStatus { get; set; }
}

public enum OrderStatus
{
    Created,
    Wait,
    Work,
    Ready,
    Completed
}