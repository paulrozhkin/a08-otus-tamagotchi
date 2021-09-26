using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tamagotchi.Orders.Microservice.WebApi.Controllers.Restaurants.Dto;

namespace Tamagotchi.Orders.Microservice.WebApi.Controllers.Restaurants
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        /// <summary>
        /// Get all restaurants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<RestaurantDto> GetAllRestaurants()
        {
            return new List<RestaurantDto>()
            {
                new() {Id = 1, Name = "Idiot", Address = "Embankment river Moyka, 82, St Petersburg"},
                new() {Id = 2, Name = "Terrassa", Address = "Kazanskaya St, 3А, St Petersburg"},
                new() {Id = 3, Name = "Levopravo Spb", Address = "Moshkov Pereulok, 4"}
            };
        }
    }
}