using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Common;
using Restaurants.Application.Users.Commands.AssignRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPut("user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }


        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AssignUserRole([FromBody] AssignRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }


        // DELETE api/<IdentityController>/5
        [HttpDelete]
        [Authorize(Roles = UserRoles.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UnassignUserRole([FromBody] UnassignUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
