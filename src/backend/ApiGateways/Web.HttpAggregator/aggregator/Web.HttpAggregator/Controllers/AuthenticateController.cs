using System.Collections.Generic;
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
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IUserService _userService;

        public AuthenticateController(ILogger<AuthenticateController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateResponse))]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.AuthenticateAsync(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
        public async Task<ActionResult> RegisterNewClientAsync(RegistrationRequest user)
        {
            try
            {
                var newUser = new CreateUserRequest()
                {
                    Name = user.Name,
                    Password = user.Password,
                    Roles = new List<string>() {Roles.Client},
                    Username = user.Username
                };

                var createdUser = await _userService.CreateUserAsync(newUser);
                return CreatedAtAction("RegisterNewClient", new { id = createdUser.Id }, createdUser);
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
