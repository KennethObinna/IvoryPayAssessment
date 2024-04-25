using IvoryPayAssessment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class ProductCategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
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
             

            source = new ValidationResponse();
            return true;
        }
    }
    public class ProductCategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Id {  get; set; }
    }
}
