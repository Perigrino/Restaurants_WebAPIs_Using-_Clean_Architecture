using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;


namespace Restaurants.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
    {
        // GET: api/<RestaurantController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurant = await restaurantService.GetAllRestaurants();
            return Ok(restaurant);
        }

        // GET api/<RestaurantController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById (Guid id)
        {
            var restaurant = await restaurantService.GetAllRestaurantById(id);
            if ( restaurant != null)
                return Ok(restaurant);
            
            return NotFound();
        }

        // POST api/<RestaurantController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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