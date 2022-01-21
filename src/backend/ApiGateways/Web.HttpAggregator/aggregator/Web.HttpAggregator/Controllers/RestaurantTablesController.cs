using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Exceptions;
using Domain.Core.Models;
using Infrastructure.Core.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.HttpAggregator.Models;
using Web.HttpAggregator.Models.QueryParameters;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [ApiController]
    [Route("api/v1/restaurants/{restaurantId:Guid}/tables")]
    [Authorize(Roles = Roles.Administrator)]
    public class RestaurantTablesController : ControllerBase
    {
        private readonly ILogger<RestaurantTablesController> _logger;
        private readonly ITablesService _tablesService;

        public RestaurantTablesController(ILogger<RestaurantTablesController> logger, ITablesService tablesService)
        {
            _logger = logger;
            _tablesService = tablesService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<TableResponse>))]
        public async Task<ActionResult> GetTablesAsync(
            Guid restaurantId,
            [FromQuery] QueryStringParameters parameters)
        {
            var table = await _tablesService.GetTablesAsync(restaurantId, parameters.PageNumber, parameters.PageSize);
            return Ok(table);
        }

        [HttpGet("{tableId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTableByIdAsync(Guid tableId)
        {
            try
            {
                var table = await _tablesService.GetTableByIdAsync(tableId);
                return Ok(table);
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TableResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateTableAsync(Guid restaurantId, TableRequest table)
        {
            try
            {
                var createdTable = await _tablesService.CreateTableAsync(restaurantId, table);
                return CreatedAtAction("CreateTable", new {id = createdTable.Id}, createdTable);
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

        [HttpPut("{tableId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateTableAsync(Guid restaurantId, Guid tableId, TableRequest table)
        {
            try
            {
                var updatedTable = await _tablesService.UpdateTableAsync(restaurantId, tableId, table);
                return Ok(updatedTable);
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

        [HttpDelete("{tableId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTableAsync(Guid tableId)
        {
            try
            {
                await _tablesService.DeleteTableAsync(tableId);
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