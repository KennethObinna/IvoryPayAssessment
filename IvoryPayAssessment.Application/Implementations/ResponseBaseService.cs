using IvoryPayAssessment.Application.Interfacses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Implementations
{
    public class ResponseBaseService
    {
        private readonly IMessageProvider _messageProvider;
        private readonly string _language = string.Empty;
        private readonly IHttpContextAccessor _httpContext;
        public ResponseBaseService(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
             
        }
        public ServerResponse<T> SetErrorLogin<T>(ServerResponse<T> response, string responseCode, int count, string language)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = _messageProvider.GetMessage(responseCode, language)

            };
            response.Error.responseDescription = response.Error.responseDescription.Replace("{rem}", count.ToString());
            return response;
        }
        public ServerResponse<T> SetErrorGeneric<T>(ServerResponse<T> response, string responseCode, Dictionary<string, string> constants)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = _messageProvider.GetMessage(responseCode, _language)

            };
            string err = response.Error.responseDescription;
            foreach (var i in constants)
            {
                err = err.Replace(i.Key, i.Value);
            }
            response.Error.responseDescription = err;
            return response;
        }
        public ServerResponse<T> SetError<T>(ServerResponse<T> response, string responseCode, string language)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = _messageProvider.GetMessage(responseCode, language)

            };
            return response;
        }
        public ServerResponse<T> SetIdentityError<T>(ServerResponse<T> response, string responseCode, string message, string language)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = $"{_messageProvider.GetMessage(responseCode, language)} {message}"

            };
            return response;
        }
        public ServerResponse<T> SetErrorData<T>(ServerResponse<T> response, T data, string responseCode, string language)
        {
            response.SuccessMessage = _messageProvider.GetMessage(responseCode, language);
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }
 
        
        public ServerResponse<T> SetError<T>(ServerResponse<T> response, string responseCode, string message, string language)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = message

            };
            return response;
        }
        public ServerResponse<T> SetError<T>(ServerResponse<T> response, string responseCode)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription =  _messageProvider.GetMessage(responseCode, _language)

            };
            return response;
        }
        public ServerResponse<T> SetSuccess<T>(ServerResponse<T> response, T data, string responseCode, string language)
        {
            response.SuccessMessage = _messageProvider.GetMessage(responseCode, language);
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }
        public ServerResponse<T> SetSuccess<T>(ServerResponse<T> response, T data, string responseCode)
        {
            response.SuccessMessage = _messageProvider.GetMessage(responseCode, _language);
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }
        
        public ServerResponse<T> SetSuccessWithStatus<T>(ServerResponse<T> response, T data, string responseCode, string status, string language)
        {
            response.SuccessMessage = _messageProvider.GetMessage(responseCode, language)?.Replace("{status}", status);
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }

       
        public ServerResponse<T> SetErrorWithStatus<T>(ServerResponse<T> response, string responseCode, string status, string language)
        {
            response.Error = new ErrorResponse
            {
                responseCode = responseCode,
                responseDescription = _messageProvider.GetMessage(responseCode, language)?.Replace("{status}", status)

            };
            return response;
        }


    }
}
