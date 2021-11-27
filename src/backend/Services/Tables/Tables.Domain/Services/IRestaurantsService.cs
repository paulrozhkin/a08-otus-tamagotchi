﻿using Domain.Core.Exceptions;

namespace Tables.Domain.Services
{
    public interface IRestaurantsService
    {
        /// <summary>
        /// Check restaurant exist by id.
        /// </summary>
        /// <param name="restaurantId">Restaurant id.</param>
        /// <exception cref="EntityNotFoundException">Throw exception if restaurant not exist.</exception>
        /// <returns>Void if restaurant exist.</returns>
        Task CheckRestaurantExistAsync(Guid restaurantId);
    }
}
