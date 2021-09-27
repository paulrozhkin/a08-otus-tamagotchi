using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public Task<PaginationResponse<RestaurantResponse>> GetRestaurantsAsync(
            [FromQuery] RestaurantParameters parameters)
        {
            return _restaurantService.GetRestaurantsAsync(parameters.PageNumber, parameters.PageSize,
                parameters.Address);
        }

        [HttpGet("{restaurantId:int}")]
        public async Task<ActionResult<RestaurantResponse>> GetRestaurantByIdAsync(int restaurantId)
        {
            try
            {
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(restaurantId);
                return restaurant;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurant(CreateRestaurantRequest restaurant)
        {
            var createdRestaurant = await _restaurantService.CreateRestaurant(restaurant);
            return CreatedAtAction(nameof(CreateRestaurant), new {id = createdRestaurant.Id}, createdRestaurant);
        }
    }
}