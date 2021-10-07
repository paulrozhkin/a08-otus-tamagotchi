using Microsoft.AspNetCore.Mvc;
using System;

namespace Tamagotchi.Orders.Microservice.WebApi.Controllers.Orders
{
    /// <summary>
    /// Orders controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        [HttpPut]
        public void Order(int restaurantId)
        {
            Console.WriteLine($"You order restaurant with id {restaurantId}");
        }
    }
}