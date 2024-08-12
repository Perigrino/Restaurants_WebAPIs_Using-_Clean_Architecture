using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefsGuy.Contracts.Responses;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Commands.DeleteDishes;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurantQuery;
using Restaurants.Application.Dishes.Queries.GetDishForRestaurant;

namespace Restaurants.API.Controller
{
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class DishController(IMediator mediator) : ControllerBase
    {
        // GET: api/<DishController>
        [HttpGet]
        public async Task<ActionResult> GetAllDishesForRestaurant([FromRoute] Guid restaurantId)
        {
            var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
            
            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 200,
                Message = $"All dishes for the restaurant with ID {restaurantId} have been successfully retrieved.",
                Data = dishes
            };
            return Ok(finalResponse);

        }

        // GET api/<DishController>/5
        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetDishByIdForRestaurant([FromRoute] Guid restaurantId ,[FromRoute] Guid dishId)
        {
            var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            return Ok(dish);
        }

        // POST api/<DishController>
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute]Guid restaurantId, CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            await mediator.Send(command);
            return Created();
        }

        // PUT api/<DishController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDishForRestaurant ([FromRoute] Guid id, [FromBody] UpdateDishCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent(); 
        }

        // DELETE api/<DishController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDishById ([FromRoute] Guid restaurantId, [FromRoute] Guid id)
        {
            
            await mediator.Send(new DeleteDishCommand(restaurantId, id));
            return NoContent(); 
        }
        
        // DELETE api/<DishController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteDishesForRestaurant ([FromRoute] Guid restaurantId)
        {
            
            await mediator.Send(new DeleteDishesCommand(restaurantId));
            return NoContent(); 
        }
    }
}