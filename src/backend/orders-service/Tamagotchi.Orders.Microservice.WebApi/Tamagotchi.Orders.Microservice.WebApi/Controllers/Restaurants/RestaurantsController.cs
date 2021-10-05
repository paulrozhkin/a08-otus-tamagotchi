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
                new() {Id = 1, Name = "Idiot", Address = "Embankment river Moyka, 82, St Petersburg", Latitude = 59.9303268344507, Longitude = 30.30349280398871},
                new() {Id = 2, Name = "Terrassa", Address = "Kazanskaya St, 3А, St Petersburg", Latitude = 59.93374477083977, Longitude = 30.322657508730533},
                new() {Id = 3, Name = "Levopravo Spb", Address = "Moshkov Pereulok, 4", Latitude = 59.94350884303181, Longitude = 30.321775898536195}
            };
        }
    }
}