using IvoryPayAssessment.Application.Interfacses.UserAccounts;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.UserAccounts.Users
{
    [Authorize]
    [Area(nameof(UserAccounts.Users))]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {

        private readonly IUserService _userService;
      

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
          
        }
       
        /// <summary>
        /// API to view all users
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("view-all-users")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllUsers();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
        /// <summary>
        /// APi to create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServerResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterViewModel request)
        {
            var response = await _userService.CreateUser(request);

            if (response.IsSuccessful)
            {
                return Created( nameof(CreateUser),response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        /// <summary>
        /// APi to to check user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<List<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("check-user/{email}")]
        public async Task<IActionResult> IsUserExists([FromRoute]string email)
        {
            var response = await _userService.IsUserExists(email);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
    
    }
}
