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
    [Route("api/v1/restaurants/{restaurantId:Guid}/menu")]
    public class RestaurantMenuController : ControllerBase
    {
        private readonly ILogger<RestaurantMenuController> _logger;
        private readonly IMenuService _menuService;

        public RestaurantMenuController(ILogger<RestaurantMenuController> logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<MenuItemResponse>))]
        public async Task<ActionResult> GetMenuAsync(
            Guid restaurantId,
            [FromQuery] QueryStringParameters parameters)
        {
            var menu = await _menuService.GetMenuAsync(restaurantId, parameters.PageNumber, parameters.PageSize);
            return Ok(menu);
        }

        [HttpGet("{menuItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuItemResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMenuByIdAsync(Guid menuItemId)
        {
            try
            {
                var menuItem = await _menuService.GetMenuByIdAsync(menuItemId);
                return Ok(menuItem);
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MenuItemResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateMenuAsync(Guid restaurantId, MenuItemRequest menuItem)
        {
            try
            {
                var createdMenu = await _menuService.CreateMenuAsync(restaurantId, menuItem);
                return CreatedAtAction("CreateMenu", new {id = createdMenu.Id}, createdMenu);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.BadRequest, detail: e.Message,
                    title: Errors.Entities_Entity_invalid_arguments);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
        }

        [HttpPut("{menuItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuItemResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateMenuAsync(Guid restaurantId, Guid menuItemId, MenuItemRequest menuItem)
        {
            try
            {
                var updatedMenu = await _menuService.UpdateMenuAsync(restaurantId, menuItemId, menuItem);
                return Ok(updatedMenu);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.BadRequest, detail: e.Message,
                    title: Errors.Entities_Entity_invalid_arguments);
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

        [HttpDelete("{menuItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMenuAsync(Guid menuItemId)
        {
            try
            {
                await _menuService.DeleteMenuAsync(menuItemId);
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