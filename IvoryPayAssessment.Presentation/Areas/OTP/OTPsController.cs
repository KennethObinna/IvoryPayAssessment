using IvoryPayAssessment.Application.Interfacses.OTPServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvoryPayAssessment.Presentation.Areas.OTP
{
   

    [Area("OTP")]
    [Route("api/[controller]")]
    public class OTPsController : BaseController
    {
        private readonly IOTPService _oTPService;

        public OTPsController(IOTPService oTPService)
        {
            _oTPService = oTPService;
        }
        /// <summary>
        /// An API to generate otp :
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("generate-otp")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServerResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public  async Task<IActionResult> SendOTP([FromBody] OTPRequest request)
        {
         
            var response = await _oTPService.GenerateOTP(request, false);
            return Ok(response);
        }
    /// <summary>
    /// An API to validate that the OTP entered by the user is valid and is from our system
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ServerResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> VerifyOTP(ValidateOTPRequest request)
        {
         
            var response = await _oTPService.ValidateOTP(request);
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
