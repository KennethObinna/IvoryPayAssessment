using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class RestaurantDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
  

        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();
            if (string.IsNullOrEmpty(Name))
            {
                var message = $"Name {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrWhiteSpace(Address))
            {
                var message = $"Address {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Latitude < -90)
            {
                var message = $"Latitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Longitude <-180)
            {
                var message = $"Longitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
           
            source = new ValidationResponse();
            return true;
        }
    }

    public class RestaurantModelView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
   
    }

    public class FetchRestaurantDTO
    {

        public string City { get; set; }
        public int Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(City))
            {
                var message = $"City {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Latitude <= 0)
            {
                var message = $"Latitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Longitude <= 0)
            {
                var message = $"Longitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }

            source = new ValidationResponse();
            return true;
        }
    }
    public class UpdateRestaurantDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, IHttpContextAccessor httpContext)
        {
            var lang = Convert.ToString(httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE]);
            var response = new ValidationResponse();
            if (Id <= 0)
            {
                var message = $"Id {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrEmpty(Name))
            {
                var message = $"Name {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrWhiteSpace(Address))
            {
                var message = $"Address {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Latitude <= 0)
            {
                var message = $"Latitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Longitude <= 0)
            {
                var message = $"Longitude {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
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
