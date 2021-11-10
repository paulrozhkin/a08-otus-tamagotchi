using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.HttpAggregator.Models.OrderQueue;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderQueueController
        : ControllerBase
    {
        private readonly ILogger<OrderQueueController> _logger;
        private readonly IOrderQueueService _orderQueueService;

        public OrderQueueController(ILogger<OrderQueueController> logger, IOrderQueueService orderQueueService)
        {
            _logger = logger;
            _orderQueueService = orderQueueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KitchenOrderResponse>>> GetAllAsync()
        {
            var response = await _orderQueueService.GetKitchenOrders();
            return Ok(response);
        }
    }
}
