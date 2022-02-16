using System;
using DishesApi;

namespace Web.HttpAggregator.Models;

public class OrderPositionRequest
{
    public Guid Id { get; set; }

    public int Count { get; set; }
}