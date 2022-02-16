using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<OrderResponse>))]
        public async Task<ActionResult> GetOrdersAsync(
            [FromQuery] QueryStringParameters parameters)
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var orders = await _ordersService.GetOrdersAsync(parameters.PageNumber, parameters.PageSize, Guid.Parse(userId));
            return Ok(orders);
        }

        /// <summary>
        /// Order table in selected restaurant
        /// </summary>
        /// <param name="orderRequest"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]

        public async Task<ActionResult> OrderAsync([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var createdOrder = await _ordersService.BookRestaurantAsync(orderRequest, Guid.Parse(userId));
                return CreatedAtAction("Order", new { id = createdOrder.Id }, createdOrder);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int)HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
        }
    }
}