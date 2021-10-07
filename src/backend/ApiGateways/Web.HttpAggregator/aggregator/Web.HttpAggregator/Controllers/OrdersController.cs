using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.HttpAggregator.Controllers
{
    /// <summary>
    /// Restaurant table orders controller
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        [HttpPut]
        public void Order([FromQuery] int restaurantId)
        {
            Console.WriteLine($"You order restaurant with id {restaurantId}");
        }
    }
}
