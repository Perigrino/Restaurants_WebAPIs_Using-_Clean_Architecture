using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
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
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllRestaurants()
        {
            var restaurants = await mediator.Send( new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }

        // GET api/<RestaurantController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] Guid id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(){ Id = id });
            if ( restaurant != null)
                return Ok(restaurant);
            
            return NotFound();
        }

        // POST api/<RestaurantController>
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant ([FromBody]CreateRestaurantCommand? command)
        {
            if (command == null)
            {
                return BadRequest("Restaurant data cannot be null.");
            }
            var restaurant = await mediator.Send(command);
            return Ok("Restaurant created successfully.");

            
        }

        // PUT api/<RestaurantController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RestaurantController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}