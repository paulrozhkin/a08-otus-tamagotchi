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
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<UserResponse>))]
        public async Task<ActionResult> GetUsersAsync(
            [FromQuery] QueryStringParameters parameters)
        {
            var users =
                await _userService.GetUsersAsync(parameters.PageNumber, parameters.PageSize);
            return Ok(users);
        }

        [HttpGet("{userId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUsersByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"User with id {userId} not found");
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: $"User with id {userId} not found");
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
        public async Task<ActionResult> CreateUserAsync(CreateUserRequest user)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction("CreateUser", new {id = createdUser.Id}, createdUser);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int) HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
        }

        [HttpPut("{userId:guid}")]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateUserAsync(Guid userId, UpdateUserRequest user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, user);
                return Ok(updatedUser);
            }
            catch (EntityAlreadyExistsException e)
            {
                _logger.LogError(e.ToString());
                return Problem(statusCode: (int)HttpStatusCode.Conflict, detail: e.Message,
                    title: Errors.Entities_Entity_already_exits);
            }
            catch (EntityNotFoundException e)
            {
                var message = e.ToString();
                _logger.LogError(message);
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: message);
            }
        }

        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = Roles.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError($"User with id {userId} not found");
                return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                    detail: $"User with id {userId} not found");
            }
        }
    }
}