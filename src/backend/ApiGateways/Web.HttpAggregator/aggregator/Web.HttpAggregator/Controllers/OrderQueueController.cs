using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Infrastructure.Core.Messages.OrderQueueMessages;
using Web.HttpAggregator.Models.OrderQueue;

namespace Web.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderQueueController
        : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public OrderQueueController(ISendEndpointProvider sendEndpointProvider, IMapper mapper)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Создать заказ для кухни
        /// </summary>        
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] KitchenOrderCreateRequest request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:new-kitchen-order"));
            await endpoint.Send(_mapper.Map<NewKitchenOrderMessage>(request));
            return Ok();
        }
    }
}