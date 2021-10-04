using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models.KitchenOrder;
using Restaurant.Core.Abstractions.Repositories;
using Restaurant.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.API.Controllers
{
    /// <summary>
    /// Заказы кухни
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KitchenOrdersController
        : ControllerBase
    {
        private readonly IRepository<KitchenOrder> _kitchenOrderRepository;
        private readonly IMapper _mapper;

        public KitchenOrdersController(
            IRepository<KitchenOrder> kitchenOrderRepository,
            IMapper mapper)
        {
            _kitchenOrderRepository = kitchenOrderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все заказы кухни
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KitchenOrderResponse>> GetAllKitchenOrdersAsync()
        {
            var orders = await _kitchenOrderRepository.GetAllAsync();
            return orders.Select(o => _mapper.Map<KitchenOrderResponse>(o)).ToList();
        }

        /// <summary>
        /// Получить заказ кухни по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<KitchenOrderResponse>> GetByIdAsync(int id)
        {
            var order = await _kitchenOrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return _mapper.Map<KitchenOrderResponse>(order);
        }
    }
}
