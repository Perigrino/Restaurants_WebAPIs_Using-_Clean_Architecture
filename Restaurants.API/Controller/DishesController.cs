using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace Restaurants.API.Controller
{
    [Route("api/restaurants/{restaurantId}/[controller]")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        // GET: api/<DishesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DishesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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