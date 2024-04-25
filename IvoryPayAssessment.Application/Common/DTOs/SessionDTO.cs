using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class SessionDTO
    {

        public SessionDTO() { DateCreated = DateTime.Now; }
        public string Token { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
      
        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();
            if (string.IsNullOrEmpty(Token))
            {
                var message = $"Token {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrWhiteSpace(UserId))
            {
                var message = $"User {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            source = new ValidationResponse();
            return true;
        }


    }
    public class UpdateSessionDTO
    {

        public string Token { get; set; }
        public string UserId { get; set; }

        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();
            if (string.IsNullOrEmpty(Token))
            {
                var message = $"Token {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrEmpty(UserId))
            {
                var message = $"User {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            source = new ValidationResponse();
            return true;
        }

    }
}
