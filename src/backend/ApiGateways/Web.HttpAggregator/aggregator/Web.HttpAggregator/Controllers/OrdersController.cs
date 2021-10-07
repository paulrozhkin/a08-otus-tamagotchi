using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    /// <summary>
    /// Restaurant table orders controller
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {

        IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }


        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        [HttpPut]
        public async Task<ActionResult> OrderAsync([FromQuery] int restaurantId)
        {
            Console.WriteLine($"You order restaurant with id {restaurantId}");

            try
            {
                await _ordersService.BookRestaurant(restaurantId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
