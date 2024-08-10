using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurantQuery;
using Restaurants.Application.Dishes.Queries.GetDishForRestaurant;

namespace Restaurants.API.Controller
{
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        // GET: api/<DishesController>
        [HttpGet]
        public async Task<ActionResult> GetAllDishesForRestaurant([FromRoute] Guid restaurantId)
        {
            var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);

        }

        // GET api/<DishesController>/5
        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetDishByIdForRestaurant([FromRoute] Guid restaurantId ,[FromRoute] Guid dishId)
        {
            var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId , dishId));
            return Ok(dish);
        }

        // POST api/<DishesController>
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute]Guid restaurantId, CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            await mediator.Send(command);
            return Created();
        }

        // PUT api/<DishesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DishesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}