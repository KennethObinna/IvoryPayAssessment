



using IvoryPayAssessment.Application.Common.Constants.ErrorBuldles;

namespace GlobalPay.UserManagement.Presentation.Controllers
{
    [Authorize]
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private readonly ITokenService _token;
        public TokenController(ITokenService token)
        {
            _token = token;
        }
        /// <summary>
        /// An API to generate a token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("generate-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServerResponse<Tokens>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GenerateToken(UserDto request)
        {
        
            var response = await _token.GenerateToken(request);

            if (response != null && response.IsSuccessful)
            {
                return Ok(response);

            }
            else
            {
                return BadRequest(response.Error);
            }
        }
        /// <summary>
        /// An API to generate a refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        [HttpPost("generate-refresh-token")]       
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServerResponse<Tokens>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GenereteRefeshToken(UserDto request)
        {
            var language = HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
            var response= await _token.GenerateRefreshToken(request);
            

            if (response != null && response.IsSuccessful)
            {
                return Ok(response);

            }
            else
            {
                return BadRequest(response.Error);
            } 
        }
        //[HttpPost("generate-refresh-token-new")]       
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(ServerResponse<Tokens>), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(ErrorResponse))]
        //public async Task<IActionResult> GenerateRefreshTokenNew()
        //{
            
        //    var response= await _token.GenerateRefreshTokenNew();
            

        //    if (response != null && response.IsSuccessful)
        //    {
        //        return Ok(response);

        //    }
        //    else
        //    {
        //        return BadRequest(response.Error);
        //    } 
        //}
    }
}
