using System.Threading.Tasks;
using Domain.Core.Exceptions;
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
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(ILogger<RestaurantsController> logger, IRestaurantService restaurantService)
        {
            _logger = logger;
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
            catch (EntityNotFoundException)
            {
                _logger.LogError($"Restaurant with id {restaurantId} not found");
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