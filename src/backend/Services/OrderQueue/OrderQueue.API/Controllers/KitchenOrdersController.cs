using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderQueue.API.Models.KitchenOrder;
using OrderQueue.Core.Abstractions.Repositories;
using OrderQueue.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderQueue.API.Controllers
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
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public KitchenOrdersController(
            IRepository<KitchenOrder> kitchenOrderRepository,
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _kitchenOrderRepository = kitchenOrderRepository;
            _publishEndpoint = publishEndpoint;
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

        /// <summary>
        /// Создать заказ для кухни
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("{orderId:int}")]
        public async Task<ActionResult> CreateAsync(int orderId)
        {
            var kitchenOrders = await _kitchenOrderRepository.FindAsync(x => x.OrderId == orderId);
            if (kitchenOrders.Any())
            {
                return BadRequest();
            }
            var newKitchenOrder = new KitchenOrder { OrderId = orderId, KitchenOrderStatusId = 1, CreateTime = DateTime.Now };
            await _kitchenOrderRepository.CreateAsync(newKitchenOrder);
            await _publishEndpoint.Publish(_mapper.Map<ExchangeModels.KitchenOrder>(newKitchenOrder));
            return Ok();
        }
    }
}
