 
using IvoryPayAssessment.Application.Interfacses.UserAccounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.UserAccounts
{
    [Authorize]
    [Area("UserAccounts")]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IAccountLogInService _accountLogIn;
        private readonly IUserService _userService;
        
        private readonly IAccountLogoutService _accountLogout;
        public AuthenticationController(IAccountLogInService accountLogIn, IAccountLogoutService accountLogout,  IUserService userService)
        {
            _accountLogIn = accountLogIn;
            _accountLogout = accountLogout;           
            _userService = userService;
        }
        /// <summary>
        /// API to login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServerResponse<LogInResponse>), StatusCodes.Status200OK)]
        // [ProducesErrorResponseType(typeof(ErrorResponse))]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest model)
        {
            var result = await _accountLogIn.LogIn(model.Email, model.Password);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return Unauthorized(result.Error);
        }
        /// <summary>
        /// API to logout user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServerResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountLogout.LogOut();

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return BadRequest(result.Error);
        }
         
     
    }
}
