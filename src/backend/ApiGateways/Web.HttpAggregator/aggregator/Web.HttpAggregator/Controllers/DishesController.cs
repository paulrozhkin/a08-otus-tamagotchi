using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Infrastructure.Core.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly ILogger<DishesController> _logger;
        private readonly IDishesService _dishesService;

        public DishesController(ILogger<DishesController> logger, IDishesService dishesService)
        {
            _logger = logger;
            _dishesService = dishesService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<DishResponse>))]
        public async Task<ActionResult> GetDishesAsync(
            [FromQuery] QueryStringParameters parameters)
        {
            var dishes = await _dishesService.GetDishesAsync(parameters.PageNumber, parameters.PageSize);
            return Ok(dishes);
        }

        [HttpGet("{dishId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DishResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetDishByIdAsync(Guid dishId)
        {
            try
            {
                var dish = await _dishesService.GetDishByIdAsync(dishId);
                return Ok(dish);
            }
            catch (EntityNotFoundException e)
            {
                var message = e.ToString();
                _logger.LogError(message);
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int) HttpStatusCode.NotFound,
                    detail: message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DishResponse))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateDishAsync(DishRequest dish)
        {
            try
            {
                var createdDish = await _dishesService.CreateDishAsync(dish);
                return CreatedAtAction("CreateDish", new {id = createdDish.Id}, createdDish);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
        }

        [HttpPut("{dishId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DishResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateDishAsync(Guid dishId, DishRequest dish)
        {
            try
            {
                var updatedDish = await _dishesService.UpdateDishAsync(dishId, dish);
                return Ok(updatedDish);
            }
            catch (EntityNotFoundException e)
            {
                var message = e.ToString();
                _logger.LogError(message);
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int) HttpStatusCode.NotFound,
                    detail: message);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
        }

        [HttpDelete("{dishId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDishAsync(Guid dishId)
        {
            try
            {
                await _dishesService.DeleteDishAsync(dishId);
                return Ok();
            }
            catch (EntityNotFoundException e)
            {
                var message = e.ToString();
                _logger.LogError(message);
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int) HttpStatusCode.NotFound,
                    detail: message);
            }
        }
    }
}