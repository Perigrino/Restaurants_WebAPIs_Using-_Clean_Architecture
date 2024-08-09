using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class RestaurantController(IMediator mediator) : ControllerBase
    {
        // GET: api/<RestaurantController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllRestaurants()
        {
            var restaurants = await mediator.Send( new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }

        // GET api/<RestaurantController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto?>> GetRestaurantById([FromRoute] Guid id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery { Id = id });
            return Ok(restaurant);
            
        }

        // POST api/<RestaurantController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRestaurant ([FromBody]CreateRestaurantCommand command)
        {
            var restaurant = await mediator.Send(command);
            return Ok("Restaurant has been created successfully");
        }
        
        // DELETE api/<RestaurantController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant ([FromRoute] Guid id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            return NoContent(); 
        }

        // PUT api/<RestaurantController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] Guid id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent();
        }
        
    }
}