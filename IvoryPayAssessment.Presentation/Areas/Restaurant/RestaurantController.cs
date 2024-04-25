using IvoryPayAssessment.Application.Interfacses.Restuarants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.Restaurant
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService??throw new ArgumentException(nameof(restaurantService));
        }
        /// <summary>
        /// API to add restaurant to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
 
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RestaurantDTO request)
        {
            var result = await _restaurantService.CreateRestaurant(request);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// An API to retunr all the nearby Restaurants based on the provided value for Latitude, City,Longitude and Distance
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
      
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("fetch-restaurants")]
        public async Task<IActionResult> FetchRestaurant([FromQuery] FetchRestaurantDTO request)
        {
            var result = await _restaurantService.GetRestaurants(request);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// An API to to fetch singly restaurant based on Longitute and Latitude
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<RestaurantModelView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("fetch-restaurant")]
        public async Task<IActionResult> GetRestaurant([FromQuery] FetchRestaurantDTO request)
        {
            var result = await _restaurantService.GetRestaurant(request);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// AN APi to fetch a restaurant using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<RestaurantModelView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("fetch-restaurant/{id}")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] long id)
        {
            var result = await _restaurantService.GetRestaurantById(id);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// An API to archive or delete the specified record from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var result = await _restaurantService.DeleteRestaurantsById(id);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// An api to update the restaurant recoord using the id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPut("update")]
        public async Task<IActionResult> Delete([FromBody] UpdateRestaurantDTO request)
        {
            var result = await _restaurantService.UpdateRestaurant(request);

            if (result.IsSuccessful)
            {
                return Created(nameof(Create), result);
            }
            return BadRequest(result.Error);
        }

    }
}
