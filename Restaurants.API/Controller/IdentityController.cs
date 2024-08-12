using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefsGuy.Contracts.Responses;
using Restaurants.Application.Users.Commands;

namespace Restaurants.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
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
        //
        // // DELETE api/<IdentityController>/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}