using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<OrderResponse>))]
        public async Task<ActionResult> GetOrdersAsync(
            [FromQuery] QueryStringParameters parameters)
        {
            //var dishes = await _dishesService.GetDishesAsync(parameters.PageNumber, parameters.PageSize);
            //return Ok(dishes);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="orderRequest"></param>
        [HttpPost]
        public async Task<ActionResult> OrderAsync([FromBody] OrderRequest orderRequest)
        {
            throw new NotImplementedException();

            //try
            //{
            //    var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            //    await _ordersService.BookRestaurantAsync(orderRequest);
            //    return Ok();
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
        }
    }
}