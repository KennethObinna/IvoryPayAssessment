

using Azure.Core;
using IvoryPayAssessment.Application.Common.Helpers;
using Newtonsoft.Json;
using System.Globalization;

using System.Text.Json.Serialization;


namespace IvoryPayAssessment.Application.Common.Models
{

    public class RegisterViewModel
    {
        public UserType UserType { get; set; }
        public string Email { get; set; }     
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
     
     
        public DateTime DateofBirth { get; set; }
        public string  Address { get; set; }   
   
        public Gender Gender { get; set; }
        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();
          
            if (DateofBirth <= default(DateTime))
            {
                var message = $"Date of birth {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            
            if (string.IsNullOrEmpty(Email))
            {
                var message = $"Email {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
           
            if (string.IsNullOrEmpty(FirstName))
            {
                var message = $"Firtname {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            
            if (string.IsNullOrEmpty(LastName))
            {
                var message = $"Lastname {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                var message = $"PhoneNumber {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (UserType <= 0)
            {
                var message = $"User type {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
          

            source = response;
            return true;
        }
    }
 
 
 

}
