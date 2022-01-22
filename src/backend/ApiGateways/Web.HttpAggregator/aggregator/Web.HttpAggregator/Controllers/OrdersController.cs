using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    /// <summary>
    /// Restaurant table orders controller
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }


        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="orderRequest"></param>
        [HttpPost]
        public async Task<ActionResult> OrderAsync([FromBody] OrderRequest orderRequest)
        {
            Console.WriteLine($"You order restaurant with id {orderRequest.RestaurantId}");

            try
            {
                await _ordersService.BookRestaurantAsync(orderRequest.RestaurantId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}