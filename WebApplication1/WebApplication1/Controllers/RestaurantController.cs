using System.Collections.Generic;
using System.Web.Http;
using System.Net;
public class RestaurantController : ApiController
{
    //private static List<Restaurant> restaurants = new List<Restaurant>();
    private FileService fileService = new FileService();

    [HttpPost]
    [Route("api/restaurants")]
    public IHttpActionResult AddRestaurant(Restaurant restaurant)
    {
        if (restaurant == null)
            return BadRequest("Restaurant is null");

        var restaurants = fileService.LoadRestaurants();

        restaurants.Add(restaurant);

        fileService.SaveRestaurants(restaurants);

        return StatusCode(HttpStatusCode.Created);
    }

    [HttpGet]
    [Route("api/restaurants")]
    public List<Restaurant> GetRestaurants()
    {
        return fileService.LoadRestaurants();
    }
}