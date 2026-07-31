using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Common;
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
    [Authorize]
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
        public async Task<ActionResult<DishDto>> GetDishByIdForRestaurant([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
        {
            var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));

            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 200,
                Message = $"Dish with ID {dishId} has been successfully retrieved.",
                Data = dish
            };
            return Ok(finalResponse);
        }

        // POST api/<DishController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDish([FromRoute] Guid restaurantId, CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            await mediator.Send(command);

            var finalResponse = new FinalResponse<object>
            {
                StatusCode = 201,
                Message = "Dish has been created successfully.",
                Data = null
            };
            return StatusCode(StatusCodes.Status201Created, finalResponse);
        }

        // PUT api/<DishController>/5
        [HttpPut("{dishId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDishForRestaurant([FromRoute] Guid restaurantId, [FromRoute] Guid dishId, [FromBody] UpdateDishCommand command)
        {
            command.Id = dishId;
            command.RestaurantId = restaurantId;
            await mediator.Send(command);
            return NoContent();
        }

        // DELETE api/<DishController>/5
        [HttpDelete("{dishId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDishById([FromRoute] Guid restaurantId, [FromRoute] Guid dishId)
        {
            await mediator.Send(new DeleteDishCommand(restaurantId, dishId));
            return NoContent();
        }

        // DELETE api/<DishController>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDishesForRestaurant([FromRoute] Guid restaurantId)
        {
            await mediator.Send(new DeleteDishesCommand(restaurantId));
            return NoContent();
        }
    }
}
