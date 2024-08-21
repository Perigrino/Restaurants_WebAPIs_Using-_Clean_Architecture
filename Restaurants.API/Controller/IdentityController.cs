using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefsGuy.Contracts.Responses;
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
        public async Task<IActionResult> UpdateUserDetails ([FromBody] UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 204,
                Message = "User details have been updated successfully.",
                Data = null
            };
            return Ok(finalResponse);
        }
        
        
        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> AssignUserRole ([FromBody] AssignRoleCommand command)
        {
            await mediator.Send(command);
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 204,
                Message = "User role has been assigned successfully.",
                Data = null
            };
            return Ok(finalResponse);
        }
        
        
        // DELETE api/<IdentityController>/5
        [HttpDelete]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> UnassignUserRole([FromBody] UnassignUserRoleCommand command)
        {
            await mediator.Send(command);
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 204,
                Message = "User role has been removed successfully.",
                Data = null
            };
            return Ok(finalResponse);
        }
        
        
        // GET: api/<IdentityController>
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
        //
        // // GET api/<IdentityController>/5
        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return "value";
        // }
        //
        // // POST api/<IdentityController>
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // PUT api/<IdentityController>/5

        //
    
    }
}