﻿using System;
using Orders.API;
using System.Threading.Tasks;

namespace Web.HttpAggregator.Services
{
    public interface IOrdersService
    {
        Task<BookRestauranResponse> BookRestaurant(Guid restaurantId);
    }
}