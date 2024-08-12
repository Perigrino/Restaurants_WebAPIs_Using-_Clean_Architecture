using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefsGuy.Contracts.Responses;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;


namespace Restaurants.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController(IMediator mediator) : ControllerBase
    {
        // GET: api/<RestaurantController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllRestaurants()
        {
            var restaurants = await mediator.Send( new GetAllRestaurantsQuery());
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 200,
                Message = $"All restaurants have been successfully retrieved",
                Data = restaurants
            };
            return Ok(finalResponse); 
        }

        // GET api/<RestaurantController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto?>> GetRestaurantById([FromRoute] Guid id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery { Id = id });
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 200,
                Message = $"Restaurant with ID {id} has been successfully retrieved",
                Data = restaurant
            };
            
            return Ok(finalResponse);
            
        }

        // POST api/<RestaurantController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRestaurant ([FromBody]CreateRestaurantCommand command)
        {
            await mediator.Send(command);
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 201,
                Message = "Restaurant has been created successfully.",
                Data = null
            };
            return Ok(finalResponse);

        }
        
        
        // PUT api/<RestaurantController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] Guid id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 204,
                Message = $"Restaurant with ID:{id} has been updated successfully.",
                Data = null
            };
            return Ok(finalResponse);
        }
        
        
        // DELETE api/<RestaurantController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant ([FromRoute] Guid id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 204,
                Message = "Restaurant has been deleted successfully.",
                Data = null
            };
            return Ok(finalResponse);
        }


        
    }
}