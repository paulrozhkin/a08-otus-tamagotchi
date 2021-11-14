using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly ILogger<RestaurantsController> _logger;
        private readonly IRestaurantsService _restaurantsService;

        public RestaurantsController(ILogger<RestaurantsController> logger, IRestaurantsService restaurantsService)
        {
            _logger = logger;
            _restaurantsService = restaurantsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetRestaurantsAsync(
            [FromQuery] RestaurantParameters parameters)
        {
            var restaurants =
                await _restaurantsService.GetRestaurantsAsync(parameters.PageNumber, parameters.PageSize,
                    parameters.Address);
            return Ok(restaurants);
        }

        [HttpGet("{restaurantId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetRestaurantByIdAsync(Guid restaurantId)
        {
            try
            {
                var restaurant = await _restaurantsService.GetRestaurantByIdAsync(restaurantId);
                return Ok(restaurant);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"Restaurant with id {restaurantId} not found");
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: $"Restaurant with id {restaurantId} not found");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DishResponse))]
        public async Task<ActionResult> CreateRestaurantAsync(RestaurantRequest restaurant)
        {
            var createdRestaurant = await _restaurantsService.CreateRestaurantAsync(restaurant);
            return CreatedAtAction("CreateRestaurant", new {id = createdRestaurant.Id}, createdRestaurant);
        }

        [HttpPut("{restaurantId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantRequest))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateRestaurantAsync(Guid restaurantId, RestaurantRequest restaurant)
        {
            try
            {
                var updatedDish = await _restaurantsService.UpdateRestaurantAsync(restaurantId, restaurant);
                return Ok(updatedDish);
            }
            catch (EntityNotFoundException e)
            {
                var message = e.ToString();
                _logger.LogError(message);
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: message);
            }
        }

        [HttpDelete("{restaurantId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRestaurantAsync(Guid restaurantId)
        {
            try
            {
                await _restaurantsService.DeleteRestaurantAsync(restaurantId);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"Restaurant with id {restaurantId} not found");
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: $"Restaurant with id {restaurantId} not found");
            }
        }
    }
}