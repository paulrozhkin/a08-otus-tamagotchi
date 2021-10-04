using Microsoft.AspNetCore.Mvc;

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
            // TODO call gRPC OrderService logic 
        }
    }
}