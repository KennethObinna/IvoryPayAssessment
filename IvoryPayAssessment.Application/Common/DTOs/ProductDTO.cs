using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
       
        public string Description { get; set; }
        public long? ProductCategoryId { get; set; }
        public decimal Price {  get; set; }
        public int Quantity { get; set; }

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
            if (string.IsNullOrEmpty(Description))
            {
                var message = $"Description {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Quantity<=0)
            {
                var message = $"Quantity {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (ProductCategoryId <=0)
            {
                var message = $"Product Category {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (Price <=100)
            {
                var message = $"Price {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, lang)}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }

            source = new ValidationResponse();
            return true;
        }
    }
    public class ProductModelView
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public long? ProductCategoryId { get; set; }
        public long Id {  get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }


}
